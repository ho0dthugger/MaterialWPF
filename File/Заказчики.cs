//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MaterialWPF.File
{
    using System;
    using System.Collections.Generic;
    
    public partial class Заказчики
    {
        public Заказчики()
        {
            this.Заказы = new HashSet<Заказы>();
        }
    
        public int ID_Заказчика { get; set; }
        public string Имя { get; set; }
        public string Адрес { get; set; }
        public string Описание { get; set; }
    
        public virtual ICollection<Заказы> Заказы { get; set; }
    }
}
