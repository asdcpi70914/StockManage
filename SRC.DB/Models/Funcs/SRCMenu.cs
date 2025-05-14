using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SRC.DB.Models.Funcs
{

    public class SRCMenu
    {
        public int? func_id { get; set; }
        public int? role_id { get; set; }
        public string url { get; set; }
        public short? weight { get; set; }

        public string name { get; set; }
        public string type { get; set; }
        public int pid { get; set; }
        public string icon { get; set; }

        public int? parentId { get; set; }

        [JsonPropertyName("checked")]
        public bool isChecked { get; set; }

        public int? level { get; set; }

        public bool hasChild { get; set; }
        public bool hasParent { get; set; }

        public IList<SRCMenu> child { get; set; }

        public IList<SRCMenu> ParseFuncItems(IList<SRCMenu> funcItems)
        {
            IList<SRCMenu> result = GetParentList(funcItems);
            ParseChild(funcItems, result);

            return result;
        }

        public IList<SRCMenu> ParseTypeItems(string type, IList<SRCMenu> funcItems, bool doSplitName = false)
        {
            //todo log
            //TPRLoger log = new TPRLoger();

            IList<SRCMenu> funcsForType = FiltFuncsByType(type, funcItems);

            IList<SRCMenu> result = GetParentList(funcsForType);


            ParseChild(funcsForType, result, doSplitName);

            return result;
        }

        public IList<SRCMenu> FiltFuncsByType(string typeName, IList<SRCMenu> funcItems)
        {
            IList<SRCMenu> result = new List<SRCMenu>();

            if (string.IsNullOrEmpty(typeName))
            {
                foreach (SRCMenu item in funcItems.ToList())
                {
                    result.Add(item);
                }
            }
            else
            {
                foreach (SRCMenu item in funcItems.ToList())
                {
                    if (item.type == typeName)
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        private IList<SRCMenu> GetParentList(IList<SRCMenu> funcItems)
        {
            IList<SRCMenu> result = new List<SRCMenu>();

            foreach (SRCMenu item in funcItems.ToList())
            {
                if (!item.parentId.HasValue)
                {
                    item.level = 1;
                    result.Add(item);
                    funcItems.Remove(item);
                }
            }

            return result;
        }


        /// <summary>
        /// 取得此URL的父到仔清單
        /// </summary>
        /// <param name="funcItems"></param>
        /// <returns></returns>
        public IList<SRCMenu> GetMenuPath(string url, IList<SRCMenu> funcItems)
        {
            IList<SRCMenu> result = new List<SRCMenu>();

            SRCMenu nextMenu = null;
            SRCMenu preMenu = null;
            nextMenu = funcItems.Where(m => m.url.ToUpper() == url.ToUpper()).FirstOrDefault();

            while (nextMenu != null)
            {
                result.Add(nextMenu);
                if (nextMenu.hasParent)
                {
                    preMenu = nextMenu;

                    nextMenu = funcItems.Where(m => m.pid == nextMenu.parentId).FirstOrDefault();

                    if (preMenu != null && nextMenu != null && preMenu.pid == nextMenu.pid)
                    {
                        //預防設定錯誤導致死迴圈
                        break;
                    }
                }
                else
                {
                    nextMenu = null;
                }
            }



            //foreach (SRCMenu item in funcItems.ToList())
            //{
            //    if (!item.parentId.HasValue)
            //    {
            //        item.level = 1;
            //        result.Add(item);
            //        funcItems.Remove(item);
            //    }
            //}

            return result.Reverse().ToList();
        }

        private void ParseChild(IList<SRCMenu> childItems, IList<SRCMenu> parentItems, bool doSplitName = false)
        {
            if (childItems.Count == 0 || parentItems.Count == 0)
            {
                if (parentItems.Count == 0)
                {
                    //todo log
                    //TPRLoger log = new TPRLoger();
                    //log.Warn($"ParseChild Warning.{JsonConvert.SerializeObject(childItems, Formatting.Indented)}");
                }

                return;
            }

            Lazy<List<SRCMenu>> nextParent = new Lazy<List<SRCMenu>>();

            foreach (SRCMenu parentItem in parentItems)
            {
                foreach (SRCMenu subItem in childItems.ToList())
                {
                    if (subItem.parentId.Value == parentItem.pid)
                    {
                        if (parentItem.child == null) parentItem.child = new List<SRCMenu>();

                        parentItem.hasChild = true;
                        subItem.hasParent = true;
                        subItem.level = parentItem.level + 1;

                        //切字,ex: 系統帳號-新增 ,取得新增兩個字即可
                        if (doSplitName && !string.IsNullOrWhiteSpace(subItem.name))
                        {
                            string[] splitName = subItem.name.Split("-");

                            if (splitName.Length > 1 && !string.IsNullOrWhiteSpace(splitName[1]))
                            {
                                subItem.name = splitName[1];
                            }
                            else
                            {
                                subItem.name = splitName[0];
                            }
                        }

                        parentItem.child.Add(subItem);
                        nextParent.Value.Add(subItem);


                        childItems.Remove(subItem);


                    }
                }
            }

            ParseChild(childItems, nextParent.Value, doSplitName);

        }

        public LinkedList<SRCMenu> GetAllParent(IList<SRCMenu> menuList, long pid)
        {
            if (menuList == null) return null;

            LinkedList<SRCMenu> parentMenuList = new LinkedList<SRCMenu>();
            IList<SRCMenu> tempMenuList = null;// menuList.Where(m => m.pid == pid).ToList();

            //if (tempMenuList.Count() == 0) return null;

            //parentMenuList.AddFirst(tempMenuList[0]);

            long? checkedPid = pid;


            while (true)
            {
                if (!checkedPid.HasValue) break;

                tempMenuList = menuList.Where(m => m.pid == checkedPid).ToList();

                if (tempMenuList.Count() == 0) break;

                parentMenuList.AddFirst(tempMenuList[0]);
                checkedPid = tempMenuList[0].parentId;

            }


            return parentMenuList;
        }

    }
}
