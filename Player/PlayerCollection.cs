/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.opensource.org/licenses/ecl2.php
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace MCForge
{

    public class PlayerCollection : List<Player>, ITypedList
    {
        private IPlayerViewBuilder __viewBuilder; 

        protected IPlayerViewBuilder _viewBuilder
        {
            get
            {
                return __viewBuilder;
            }
            set
            {
                __viewBuilder = value;
            }
        }

        public PlayerCollection(IPlayerViewBuilder viewBuilder)
        {
            _viewBuilder = viewBuilder;
        }

        #region ITypedList Members

        private PropertyDescriptorCollection __props; 

        protected PropertyDescriptorCollection _props
        {
            get
            {
                return __props;
            }
            set
            {
                __props = value;
            }
        }

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            if (_props == null)
            {
                _props = _viewBuilder.GetView();
            }
            return _props;
        }

        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return ""; // was used by 1.1 datagrid
        }

        #endregion
    }

    public interface IPlayerViewBuilder
    {
        PropertyDescriptorCollection GetView();
    }




    public delegate object PlayerMethodDelegate(Player player);

    public class PlayerMethodDescriptor : PropertyDescriptor
    {
        private PlayerMethodDelegate __method; 

        protected PlayerMethodDelegate _method
        {
            get
            {
                return __method;
            }
            set
            {
                __method = value;
            }
        }
        private Type __methodReturnType; 

        protected Type _methodReturnType
        {
            get
            {
                return __methodReturnType;
            }
            set
            {
                __methodReturnType = value;
            }
        }

        public PlayerMethodDescriptor(string name, PlayerMethodDelegate method,
         Type methodReturnType)
            : base(name, null)
        {
            _method = method;
            _methodReturnType = methodReturnType;
        }

        public override object GetValue(object component)
        {
            return _method((Player)component);
        }

        public override Type ComponentType
        {
            get { return typeof(Player); }
        }

        public override Type PropertyType
        {
            get { return _methodReturnType; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component) { }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        public override void SetValue(object component, object value) { }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
