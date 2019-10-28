using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Inventory.Common;
using Inventory.Data;
using Inventory.Data.Services;
using Inventory.Services;

using Microsoft.EntityFrameworkCore;

namespace Inventory.ViewModels
{
    public class MigrateViewModel : ViewModelBase
    {
        private IImageService ImageService { get; }

        public MigrateViewModel(IImageService imageService, ICommonServices commonServices) : base(commonServices)
        {
            ImageService = imageService;
        }

        public Result Result { get; private set; }

        private string _progressStatus = null;
        public string ProgressStatus
        {
            get => _progressStatus;
            set => Set(ref _progressStatus, value);
        }

        private string _message = null;
        public string Message
        {
            get { return _message; }
            set { if (Set(ref _message, value)) NotifyPropertyChanged(nameof(HasMessage)); }
        }

        public bool HasMessage => _message != null;

        private string _primaryButtonText;
        public string PrimaryButtonText
        {
            get => _primaryButtonText;
            set => Set(ref _primaryButtonText, value);
        }

        private string _secondaryButtonText = "Cancel";
        public string SecondaryButtonText
        {
            get => _secondaryButtonText;
            set => Set(ref _secondaryButtonText, value);
        }

        public async Task ExecuteAsync(string connectionString)
        {
            try
            {
                ProgressStatus = "Выполнение миграции...";
                using (var db = new SQLServerDb(connectionString))
                {
                    await db.Database.MigrateAsync();
                    Message = $"Применение миграции успешное.";
                    Result = Result.Ok("Применение миграции завершено.");
                }
            }
            catch (Exception ex)
            {
                Result = Result.Error("Error applying migration. See details in Activity Log");
                Message = $"Error applying migration: {ex.Message}";
                LogException("Settings", "Applying migration", ex);
            }
            PrimaryButtonText = "Ok";
            SecondaryButtonText = null;
        }

        public async Task CopyProductImagesAsync(string connectionString)
        {
            try
            {
                ProgressStatus = "Копирование изображений продуктов...";

                int thumbnailWidth = await ImageService.GetDefaultThumbnailWidthAsync();
                int thumbnailHeight = await ImageService.GetDefaultThumbnailHeightAsync();

                using (var db = new SQLServerDb(connectionString))
                {
                    var dishes = await ReadEntitiesFromJsonAsync<List<Dish>>(nameof(db.Dishes));
                    var sqlDishes = await db.Dishes.ToListAsync();
                    foreach (var sqlDish in sqlDishes)
                    {
                        var dish = dishes.FirstOrDefault(x => x.Id == sqlDish.Id);
                        sqlDish.Picture = dish.Picture;
                        sqlDish.Thumbnail = await ImageService.ResizeAsync(dish.Picture, thumbnailWidth, thumbnailHeight);
                    }
                    dishes.Clear();

                    var garnishes = await ReadEntitiesFromJsonAsync<List<DishGarnish>>(nameof(db.DishGarnishes));
                    var sqlGarnishes = await db.DishGarnishes.ToListAsync();
                    foreach (var sqlGarnish in sqlGarnishes)
                    {
                        var garnish = garnishes.FirstOrDefault(x => x.Id == sqlGarnish.Id);
                        sqlGarnish.Picture = garnish.Picture;
                        sqlGarnish.Thumbnail = await ImageService.ResizeAsync(garnish.Picture, thumbnailWidth, thumbnailHeight);
                    }
                    garnishes.Clear();

                    var ingredients = await ReadEntitiesFromJsonAsync<List<DishIngredient>>(nameof(db.DishIngredients));
                    var sqlIngredients = await db.DishIngredients.ToListAsync();
                    foreach (var sqlIngredient in sqlIngredients)
                    {
                        var ingredient = ingredients.FirstOrDefault(x => x.Id == sqlIngredient.Id);
                        sqlIngredient.Picture = ingredient.Picture;
                        sqlIngredient.Thumbnail = await ImageService.ResizeAsync(ingredient.Picture, thumbnailWidth, thumbnailHeight);
                    }
                    ingredients.Clear();

                    await db.SaveChangesAsync();
                }
                Message = "Копирование завершено";
                Result = Result.Ok(Message);
            }
            catch (Exception ex)
            {
                Result = Result.Error("Ошибка при копировании изображений продуктов");
                Message = $"Ошибка при копировании изображений продуктов: {ex.Message}";
                LogException("Settings", "Applying migration: product images", ex);
            }
            PrimaryButtonText = "Ok";
            SecondaryButtonText = null;
        }

        private async Task<T> ReadEntitiesFromJsonAsync<T>(string propertyName)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(SQLServerDb));
            string resourceName = $"Inventory.Data.JsonFiles.{propertyName}.json";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var jsonString = await reader.ReadToEndAsync();
                    var entities = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
                    return entities;
                }
            }
        }
    }
}
