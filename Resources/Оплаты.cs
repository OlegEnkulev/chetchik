//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace chetchik.Resources
{
    using System;
    using System.Collections.Generic;
    
    public partial class Оплаты
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Оплаты()
        {
            this.Квитанция = new HashSet<Квитанция>();
        }
    
        public int Айди { get; set; }
        public int АйдиСтатуса { get; set; }
        public int Цена { get; set; }
        public int АйдиСчета { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Квитанция> Квитанция { get; set; }
        public virtual Статус Статус { get; set; }
        public virtual Счета Счета { get; set; }
    }
}
