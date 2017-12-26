using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using NHibernate;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using NHibernate.Linq;
using FluentNHibernate.Automapping;
using NHibernate.Tool.hbm2ddl;

namespace rpbd_lw2
{
    namespace Model
    {
        public class Holder
        {
            public virtual int Id { get; set; }
            public virtual char Sector { get; set; }
            public virtual int Number { get; set; }

            // override object.Equals
            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                return Sector == (obj as Holder).Sector && Number==(obj as Holder).Number;
            }

            // override object.GetHashCode
            public override int GetHashCode()
            {
                // TODO: write your implementation of GetHashCode() here

                return (Sector.GetHashCode() * 228) * Number.GetHashCode();
            }

            public override string ToString()
            {
                return Sector.ToString() + Convert.ToString(Number);
            }
        }

        public class Parked
        {
            public virtual int Id { get; set; }
            public virtual Holder holder { get; set; }
            public virtual string Car { get; set; }
            public virtual string Number { get; set; }
            public virtual string Color { get; set; }
            public virtual DateTime Entry { get; set; }

            public override string ToString()
            {
                return $"{Entry.ToString()} | {holder}: {Car}, цвет {Color}, Регистрационный номер: {Number}";
            }
        }

        public class Specials
        {
            public virtual int ID { get; set; }
            public virtual string Number { get; set; }
            public virtual Holder holder { get; set; }
        }
    }

    public static class Maps
    {
        public class HolderMap: ClassMap<Model.Holder>
        {
            public HolderMap()
            {
                Id(x => x.Id);
                Map(x => x.Sector);
                Map(x => x.Number);
                Table("holder");
            }
        }

        public class ParkedMap: ClassMap<Model.Parked>
        {
            public ParkedMap()
            {
                Id(x => x.Id).GeneratedBy.Sequence("parked_id_seq");
                References(x => x.holder);
                Map(x => x.Car);
                Map(x => x.Number);
                Map(x => x.Color);
                Map(x => x.Entry);
                Table("parked");
            }
        }
        
        public class SpecialsMap: ClassMap<Model.Specials>
        {
            public SpecialsMap()
            {
                Id(x => x.ID).GeneratedBy.Sequence("specialcars_num_seq"); ;
                Map(x => x.Number);
                References(x => x.holder);
                Table("specialcars");
            }
        }
    }
}
