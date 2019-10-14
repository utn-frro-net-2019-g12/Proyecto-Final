using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation.Desktop.WPF.Models {
    public class WpfDepartamentoModel {
        public int Id { get; set; }

        public string Name { get; set; }

        public override bool Equals(object o)
        {
            var item = o as WpfDepartamentoModel;

            if(item == null)
            {
                return false;
            }

            return this.Id.Equals(item.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
