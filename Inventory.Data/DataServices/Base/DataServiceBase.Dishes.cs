#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Inventory.Data.Services
{
    partial class DataServiceBase
    {
        #region Dishes
        public async Task<Dish> GetDishAsync(int id)
        {
            return await _dataSource.Dishes
                .Include(dish => dish.MenuFolder)
                .Include(dish => dish.TaxType)
                .FirstOrDefaultAsync(dish => dish.Id == id);
        }

        public async Task<Dish> GetDishAsync(Guid rowGuid)
        {
            return await _dataSource.Dishes
                .Include(dish => dish.MenuFolder)
                .Include(dish => dish.TaxType)
                .FirstOrDefaultAsync(dish => dish.RowGuid == rowGuid);
        }

        public async Task<IList<Dish>> GetDishesAsync(int skip, int take, DataRequest<Dish> request)
        {
            IQueryable<Dish> items = GetDishes(request);

            // Execute
            var records = await items.Skip(skip).Take(take)
                .Select(dish => new Dish
                {
                    Id = dish.Id,
                    RowGuid = dish.RowGuid,
                    CreatedOn = dish.CreatedOn,
                    LastModifiedOn = dish.LastModifiedOn,
                    Barcode = dish.Barcode,
                    Description = dish.Description,
                    EnergyValue = dish.EnergyValue,
                    IsExcise = dish.IsExcise,
                    Name = dish.Name,
                    DishUnitGuid = dish.DishUnitGuid,
                    MenuFolderGuid = dish.MenuFolderGuid,
                    Price = dish.Price,
                    TaxTypeGuid = dish.TaxTypeGuid,
                    SearchTerms = dish.SearchTerms,
                    Thumbnail = dish.Thumbnail,
                    UnitCount = dish.UnitCount
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        public async Task<IList<Dish>> GetDishKeysAsync(int skip, int take, DataRequest<Dish> request)
        {
            IQueryable<Dish> items = GetDishes(request);

            // Execute
            var records = await items.Skip(skip).Take(take)
                .Select(r => new Dish
                {
                    Id = r.Id,
                })
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<Dish> GetDishes(DataRequest<Dish> request)
        {
            IQueryable<Dish> items = _dataSource.Dishes
                .Include(dish => dish.MenuFolder)
                .Include(dish => dish.TaxType);

            // Query
            if (!string.IsNullOrEmpty(request.Query))
            {
                items = items.Where(r => r.SearchTerms.Contains(request.Query.ToLower()));
            }
            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            // Order By
            if (request.OrderBy != null)
            {
                items = items.OrderBy(request.OrderBy);
            }
            if (request.OrderByDesc != null)
            {
                items = items.OrderByDescending(request.OrderByDesc);
            }

            return items;
        }

        public async Task<int> GetDishesCountAsync(DataRequest<Dish> request)
        {
            IQueryable<Dish> items = _dataSource.Dishes
                .Include(dish => dish.MenuFolder)
                .Include(dish => dish.TaxType);

            // Query
            if (!string.IsNullOrEmpty(request.Query))
            {
                items = items.Where(r => r.SearchTerms.Contains(request.Query.ToLower()));
            }
            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            return await items.CountAsync();
        }

        public async Task<int> UpdateDishAsync(Dish dish)
        {
            if (dish.Id > 0)
            {
                _dataSource.Entry(dish).State = EntityState.Modified;
            }
            else
            {
                // TODO: UIDGenerator
                //dish.Id = UIDGenerator.Next(6).ToString();
                dish.CreatedOn = DateTime.UtcNow;
                _dataSource.Entry(dish).State = EntityState.Added;
            }
            dish.LastModifiedOn = DateTime.UtcNow;
            dish.SearchTerms = dish.BuildSearchTerms();
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteDishesAsync(params Dish[] dishes)
        {
            _dataSource.Dishes.RemoveRange(dishes);
            return await _dataSource.SaveChangesAsync();
        }
        #endregion

        #region DishGarnish
        public async Task<DishGarnish> GetDishGarnishAsync(int id)
        {
            return await _dataSource.DishGarnishes
                .FirstOrDefaultAsync(garnish => garnish.Id == id);
        }

        public async Task<DishGarnish> GetDishGarnishAsync(Guid dishGarnishGuid)
        {
            return await _dataSource.DishGarnishes
                .FirstOrDefaultAsync(garnish => garnish.RowGuid == dishGarnishGuid);
        }

        public async Task<IList<DishGarnish>> GetDishGarnishesAsync(Guid dishGuid)
        {
            return await _dataSource.DishGarnishes
                .Where(garnish => garnish.DishGuid == dishGuid)
                .OrderBy(garnish => garnish.RowPosition)
                .ToListAsync();
        }

        public async Task<IList<DishGarnish>> GetDishGarnishesAsync()
        {
            return await _dataSource.DishGarnishes
                .OrderBy(garnish => garnish.Name)
                .ToListAsync();
        }

        public async Task<int> UpdateDishGarnishAsync(DishGarnish dishGarnish)
        {
            if (dishGarnish.Id > 0 || dishGarnish.RowGuid != Guid.Empty)
            {
                _dataSource.Entry(dishGarnish).State = EntityState.Modified;
            }
            else
            {
                _dataSource.Entry(dishGarnish).State = EntityState.Added;
            }
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteDishGarnishesAsync(params DishGarnish[] dishGarnishes)
        {
            _dataSource.DishGarnishes.RemoveRange(dishGarnishes);
            return await _dataSource.SaveChangesAsync();
        }
        #endregion

        #region DishIngredient
        public async Task<DishIngredient> GetDishIngredientAsync(int id)
        {
            return await _dataSource.DishIngredients
                .FirstOrDefaultAsync(ingredient => ingredient.Id == id);
        }

        public async Task<DishIngredient> GetDishIngredientAsync(Guid dishIngredientGuid)
        {
            return await _dataSource.DishIngredients
                .FirstOrDefaultAsync(ingredient => ingredient.RowGuid == dishIngredientGuid);
        }

        public async Task<IList<DishIngredient>> GetDishIngredientsAsync(Guid dishGuid)
        {
            return await _dataSource.DishIngredients
                .Where(ingredient => ingredient.DishGuid == dishGuid)
                .OrderBy(ingredient => ingredient.RowPosition)
                .ToListAsync();
        }

        public async Task<IList<DishIngredient>> GetDishIngredientsAsync()
        {
            return await _dataSource.DishIngredients
                .OrderBy(ingredient => ingredient.Name)
                .ToListAsync();
        }

        public async Task<int> UpdateDishIngredientAsync(DishIngredient dishIngredient)
        {
            if (dishIngredient.Id > 0 || dishIngredient.RowGuid != Guid.Empty)
            {
                _dataSource.Entry(dishIngredient).State = EntityState.Modified;
            }
            else
            {
                _dataSource.Entry(dishIngredient).State = EntityState.Added;
            }
            return await _dataSource.SaveChangesAsync();
        }
        
        public async Task<int> DeleteDishIngredientsAsync(params DishIngredient[] dishIngredients)
        {
            _dataSource.DishIngredients.RemoveRange(dishIngredients);
            return await _dataSource.SaveChangesAsync();
        }
        #endregion

        #region DishRecommend
        
        public async Task<IList<DishRecommend>> GetDishRecommendsAsync(Guid dishGuid)
        {
            return await _dataSource.DishRecommends
                .Include(recommend => recommend.RecommendDish)
                    .ThenInclude(recommendDish => recommendDish.TaxType)
                .Where(recommend => recommend.DishGuid == dishGuid)
                .OrderBy(recommend => recommend.RowPosition)
                .ToListAsync();
        }

        #endregion

        #region Marks

        public async Task<Mark> GetMarkAsync(Guid rowGuid)
        {
            return await _dataSource.Marks
                .FirstOrDefaultAsync(mark => mark.RowGuid == rowGuid);
        }

        public async Task<IList<Mark>> GetMarksAsync()
        {
            return await _dataSource.Marks
                .ToListAsync();
        }

        public async Task<int> UpdateMarkAsync(Mark mark)
        {
            if (mark.Id > 0 || mark.RowGuid != Guid.Empty)
            {
                _dataSource.Entry(mark).State = EntityState.Modified;
            }
            else
            {
                mark.RowGuid = Guid.NewGuid();
                _dataSource.Entry(mark).State = EntityState.Added;
            }
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteMarkAsync(Mark mark )
        {
            _dataSource.Marks.Remove(mark);
            return await _dataSource.SaveChangesAsync();
        }

        #endregion

        #region Modifier

        public async Task<Modifier> GetModifierAsync(Guid modifierGuid)
        {
            return await _dataSource.Modifiers
                .FirstOrDefaultAsync(x => x.RowGuid == modifierGuid);
        }

        public async Task<IList<Modifier>> GetModifiersAsync()
        {
            return await _dataSource.Modifiers.ToListAsync();
        }
        
        public async Task<int> UpdateModifierAsync(Modifier modifier)
        {
            if (modifier.Id <= 0)
            {
                _dataSource.Entry(modifier).State = EntityState.Added;
            }
            else
            {
                _dataSource.Entry(modifier).State = EntityState.Modified;
            }
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteModifiersAsync(params Modifier[] modifiers)
        {
            _dataSource.Modifiers.RemoveRange(modifiers);
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<IList<Modifier>> GetRelatedDishModifiersAsync(Guid dishGuid)
        {
            Guid menuFolderGuid = await GetMenuFolderGuidByDishGuid(dishGuid);

            IList<Modifier> dishModifiers = await GetDishModifiersAsync(dishGuid);
            IList<Modifier> menuModifiers = await GetMenuFolderModifiersAsync(menuFolderGuid);

            var modifiers = dishModifiers.Union(menuModifiers);
            return modifiers.OrderByDescending(modifier => modifier.IsRequired).ToList();
        }

        private async Task<Guid> GetMenuFolderGuidByDishGuid(Guid dishGuid)
        {
            Guid menuFolderGuid = await _dataSource.Dishes
                .Where(dish => dish.RowGuid == dishGuid)
                .Select(dish => dish.MenuFolderGuid)
                .FirstOrDefaultAsync();
            return menuFolderGuid;
        }

        private async Task<IList<Modifier>> GetDishModifiersAsync(Guid dishGuid)
        {
            var dishModifiers = await _dataSource.DishModifiers
                .Where(x => x.DishGuid == dishGuid)
                .Include(x => x.Modifier)
                .Select(x => new Modifier
                {
                    Id = x.Modifier.Id,
                    Name = x.Modifier.Name,
                    RowGuid = x.Modifier.RowGuid,
                    IsRequired = x.IsRequired
                })
                .ToListAsync();
            return dishModifiers;
        }

        private async Task<IList<Modifier>> GetMenuFolderModifiersAsync(Guid? menuFolderGuid)
        {
            List<Modifier> modifiers = new List<Modifier>();

            if (menuFolderGuid == null || menuFolderGuid == Guid.Empty)
            {
                return modifiers;
            }

            Guid? parentFolderGuid = menuFolderGuid;
            do
            {
                var menuModifiers = await _dataSource.MenuFolderModifiers
                    .Include(folderModifier => folderModifier.Modifier)
                    .Where(folderModifier => folderModifier.MenuFolderGuid == parentFolderGuid)
                    .Select(folderModifier => new Modifier
                    {
                        Id = folderModifier.Modifier.Id,
                        Name = folderModifier.Modifier.Name,
                        RowGuid = folderModifier.Modifier.RowGuid,
                        IsRequired = folderModifier.Modifier.IsRequired
                    })
                    .ToListAsync();
                modifiers.AddRange(menuModifiers);

                parentFolderGuid = await _dataSource.MenuFolders
                    .Where(menuFolder => menuFolder.RowGuid == parentFolderGuid)
                    .Select(menuFolder => menuFolder.ParentGuid)
                    .FirstOrDefaultAsync();
            }
            while (parentFolderGuid != null && parentFolderGuid != Guid.Empty);

            return modifiers;
        }

        #endregion

        #region DishMarks

        public async Task<DishMark> GetDishMarkAsync(Guid rowGuid)
        {
            return await _dataSource.DishMarks
                .FirstOrDefaultAsync(dishMark => dishMark.RowGuid == rowGuid);
        }

        public async Task<IList<DishMark>> GetDishMarksAsync(Guid dishGuid)
        {
            return await _dataSource.DishMarks
                .Where(garnish => garnish.DishGuid == dishGuid)
                .ToListAsync();
        }

        public async Task<int> UpdateDishMarkAsync(DishMark dishMark)
        {
            if (dishMark.Id > 0 || dishMark.RowGuid != Guid.Empty)
            {
                _dataSource.Entry(dishMark).State = EntityState.Modified;
            }
            else
            {
                dishMark.RowGuid = Guid.NewGuid();
                _dataSource.Entry(dishMark).State = EntityState.Added;
            }
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteDishMarkAsync(DishMark DishMark)
        {
            _dataSource.DishMarks.Remove(DishMark);
            return await _dataSource.SaveChangesAsync();
        }

        #endregion
    }
}
