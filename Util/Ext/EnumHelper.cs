using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Util.Ext
{
    public static class EnumHelper
    {

        public static List<SelectItem> GetPTEnumList<T>(bool MoRenAll = false)
        {

            List<SelectItem> list = new List<SelectItem>();
            if (MoRenAll)
            {
                list.Add(new SelectItem { Text = "全部", Value = -1 });
            }
            Type type = typeof(T);
            var strList = GetNamesArr<T>().ToList();
            foreach (string key in strList)
            {
                if (key.Achilles_NoContains("无"))
                {
                    string val = Enum.Format(type, Enum.Parse(type, key), "d");
                    list.Add(new SelectItem { Text = key, Value = int.Parse(val) });
                }
            }

            return list;
        }
        /// <summary>
        /// 根据枚举名称获取描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static string GetEnumDescription<T>(this string enumName)
        {

            string result = string.Empty;
            System.Reflection.FieldInfo field = typeof(T).GetField(enumName);
            if (field != null)
            {
                object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (objs == null || objs.Length == 0)
                    result = enumName;
                else
                {
                    System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
                    result = da.Description;
                }
            }
            else { result = enumName; }
            return result;
        }
        /// <summary>
        /// 获取枚举描述名称
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnmuDescription(this Enum value)
        {
            var name = value.ToString();
            var field = value.GetType().GetField(name);
            if (field == null)
            {
                return name;
            }
            var att = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);

            return att == null ? field.Name : ((DescriptionAttribute)att).Description;
        }
        /// <summary>
        /// 根据枚举获取枚举名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="status">枚举的值</param>
        /// <returns></returns>
        public static string GetEnumName2<T>(this T status)
        {
            return Enum.GetName(typeof(T), status);
        }

        /// <summary>
        /// 根据枚举的值获取枚举名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="status">枚举的值</param>
        /// <returns></returns>
        public static string GetEnumName<T>(this int status)
        {
            return Enum.GetName(typeof(T), status);
        }
        /// <summary>
        /// 获取枚举名称集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] GetNamesArr<T>()
        {
            return Enum.GetNames(typeof(T));
        }


        /// <summary>
        /// 将枚举转换成字典
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, int> GetDic<TEnum>()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            Type t = typeof(TEnum);
            var arr = Enum.GetValues(t);

            foreach (var item in arr)
            {
                dic.Add(item.ToString(), (int)item);
            }

            return dic;
        }

        /// <summary>
        /// 将枚举转换成字典
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, int> GetDic<TEnum>(bool IsDefault = false, string keyDefault = "全部", int valueDefault = -1)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            if (IsDefault) //判断是否添加默认选项
            {
                dic.Add(keyDefault, valueDefault);
            }
            Type t = typeof(TEnum);
            var arr = Enum.GetValues(t);

            foreach (var item in arr)
            {
                dic.Add(item.ToString(), (int)item);
            }

            return dic;
        }

        /// <summary>
        /// 将枚举转换成字典
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, string> GetStrDic<TEnum>(bool IsDefault = false, string keyDefault = "全部", string valueDefault = "")
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (IsDefault) //判断是否添加默认选项
            {
                dic.Add(keyDefault, valueDefault);
            }
            Type t = typeof(TEnum);
            var arr = Enum.GetValues(t);

            foreach (var item in arr)
            {
                dic.Add(item.ToString(), item.ToString());
            }

            return dic;
        }


        /// 枚举类名称
        /// 默认key值
        /// 默认value值
        /// 返回生成的字典集合
        public static List<SelectItem> GetEnumDescList<T>(string keyDefault = "请选择", int valueDefault = -1)
        {
            List<SelectItem> list = new List<SelectItem>();
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                return list;
            }
            if (!string.IsNullOrEmpty(keyDefault)) //判断是否添加默认选项
            {
                list.Add(new SelectItem { Text = keyDefault, Value = valueDefault });
            }
            string[] fieldstrs = Enum.GetNames(enumType); //获取枚举字段数组
            foreach (var item in fieldstrs)
            {
                string description = string.Empty;
                var field = enumType.GetField(item);
                object[] arr = field.GetCustomAttributes(typeof(DescriptionAttribute), true); //获取属性字段数组
                if (arr != null && arr.Length > 0)
                {
                    description = ((DescriptionAttribute)arr[0]).Description; //属性描述
                }
                else
                {
                    description = item; //描述不存在取字段名称
                }
                list.Add(new SelectItem { Text = description, Value = (int)Enum.Parse(enumType, item) });
            }
            return list;
        }




        public static List<SelectItem> GetEnumKvList<T>(string keyDefault = "请选择", int valueDefault = -1)
        {
            List<SelectItem> list = new List<SelectItem>();
            if (!string.IsNullOrEmpty(keyDefault)) //判断是否添加默认选项
            {
                list.Add(new SelectItem { Text = keyDefault, Value = valueDefault });
            }

            Type type = typeof(T);
            var strList = GetNamesArr<T>().ToList();
            foreach (string key in strList)
            {
                string val = Enum.Format(type, Enum.Parse(type, key), "d");
                list.Add(new SelectItem { Text = key, Value = int.Parse(val) });
            }
            return list;
        }




        public class SelectItem
        {

            public string Text { get; set; }
            public int Value { get; set; }

            public bool Checked { get; set; }
        }


        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="e">传入枚举对象</param>
        /// <returns>得到对应描述信息</returns>
        public static String GetEnumDesc(this Enum e)
        {
            FieldInfo EnumInfo = e.GetType().GetField(e.ToString());
            if (EnumInfo == null)
            {
                return "";
            }
            DescriptionAttribute[] EnumAttributes
                = (DescriptionAttribute[])EnumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (EnumAttributes.Length > 0)
            {
                return EnumAttributes[0].Description;
            }
            return e.ToString();
        }

        /// <summary>
        /// 将含有描述信息的枚举绑定到列表控件中
        /// </summary>
        /// <param name="enumType"></param>
        private static Dictionary<string, string> BindDesEnumToListControl(System.Type enumType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (object enumValue in Enum.GetValues(enumType))
            {
                Enum e = (Enum)enumValue;
                dic.Add(((int)enumValue).ToString(), GetEnumDesc(e));
            }
            return dic;
        }


        public static string GetEnumDesc<T>(Object obj)
        {
            obj = (T)obj;
            if (obj == null) throw new ArgumentNullException("参数不能为null");
            if (!obj.GetType().IsEnum) throw new Exception("参数类型不正确");
            FieldInfo fieldinfo = obj.GetType().GetField(obj.ToString());

            string str = string.Empty;
            Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs != null && objs.Length != 0)
            {
                DescriptionAttribute des = (DescriptionAttribute)objs[0];
                str = des.Description;
            }
            return str;
        }


        /// <summary>
        /// 根据属性描述获取枚举值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="des">属性说明</param>
        /// <returns>枚举值</returns>
        public static T GetEnum<T>(string des) where T : struct, IConvertible
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                return default(T);
            }
            T[] enums = (T[])Enum.GetValues(type);
            T temp;
            if (!Enum.TryParse(des, out temp))
            {
                temp = default(T);
            }
            for (int i = 0; i < enums.Length; i++)
            {
                string name = enums[i].ToString();
                FieldInfo field = type.GetField(name);
                object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs == null || objs.Length == 0)
                {
                    continue;
                }
                DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
                string edes = descriptionAttribute.Description;
                if (des == edes)
                {
                    temp = enums[i];
                    break;
                }
            }

            return temp;
        }
    }
}
