using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.Models
{
    public class MenuFolderTreeModel : MenuFolderModel
    {
        public List<MenuFolderTreeModel> Children { get; set; }

        public override void Merge(ObservableObject source)
        {
            if (source is MenuFolderModel model)
            {
                Merge(model);
            }
        }

        public void Merge(MenuFolderModel source)
        {
            if (source != null)
            {
                Id = source.Id;
                RowGuid = source.RowGuid;
                ParentGuid = source.ParentGuid;
                SequenceNumber = source.SequenceNumber;
                Name = source.Name;
                Description = source.Description;
                Picture = source.Picture;
                Thumbnail = source.Thumbnail;
            }
        }

        /// <summary>
        /// Представление списка меню в виде дерева
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<MenuFolderTreeModel> MenuFolderToTree(IList<MenuFolderModel> source)
        {
            List<MenuFolderTreeModel> rootTree = new List<MenuFolderTreeModel>();

            if (source == null)
            {
                return rootTree;
            }

            List<MenuFolderModel> menuFolders = source.OrderBy(s => s.SequenceNumber).ToList();
            var rootFolders = menuFolders
                .Where(menuFolder => menuFolder.ParentGuid == null || menuFolder.ParentGuid == Guid.Empty)
                .ToList();

            foreach (MenuFolderModel rootFolder in rootFolders)
            {
                var childrenTree = ChildrenTree(source, rootFolder.RowGuid);
                rootTree.Add(childrenTree);
            }
            return rootTree;
        }

        /// <summary>
        /// Рекурсивная функция - формирует узел списка
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rootGuid"></param>
        /// <returns></returns>
        private static MenuFolderTreeModel ChildrenTree(IList<MenuFolderModel> source, Guid? rootGuid)
        {
            MenuFolderTreeModel node = new MenuFolderTreeModel();

            MenuFolderModel nodeSource = source.FirstOrDefault(t => t.RowGuid == rootGuid);
            if (nodeSource != null)
                node.Merge(nodeSource);
            else
                return null;

            foreach (MenuFolderModel item in source.Where(t => t.ParentGuid == rootGuid))
            {
                if (node.Children == null)
                    node.Children = new List<MenuFolderTreeModel>();

                node.Children.Add(ChildrenTree(source, item.ParentGuid));
            }
            return node;
        }

        /// <summary>
        /// Массив ключей текущего узла и всех подчиненных
        /// </summary>
        /// <returns></returns>
        public IList<Guid> GetMenuFolderKeys()
        {
            List<Guid> keys = new List<Guid> { RowGuid };
            var childrenKeys = GetChildrenKeys(this);
            keys.AddRange(childrenKeys);
            return keys;
        }

        /// <summary>
        /// Рекурсивная функция - формирует список ключей текущего узла и всех подчиненных
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private IList<Guid> GetChildrenKeys(MenuFolderTreeModel source)
        {
            List<Guid> keys = new List<Guid>();
            if (source == null || source.Children == null)
            {
                return keys;
            }

            foreach (var children in source.Children)
            {
                keys.AddRange(GetChildrenKeys(children));
            }
            return keys;
        }
    }
}
