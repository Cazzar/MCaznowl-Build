using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace MCForge
{

    public class LevelCollection : List<Level>, ITypedList
    {
        protected ILevelViewBuilder _viewBuilder;

        public LevelCollection(ILevelViewBuilder viewBuilder)
        {
            _viewBuilder = viewBuilder;
        }

        #region ITypedList Members

        protected PropertyDescriptorCollection _props;

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

    public interface ILevelViewBuilder
    {
        PropertyDescriptorCollection GetView();
    }

    public class LevelListView : ILevelViewBuilder
    {
        public PropertyDescriptorCollection GetView()
        {
            List<PropertyDescriptor> props = new List<PropertyDescriptor>();
            LevelMethodDelegate del = delegate(Level l)
            {
                return l.name;
            };
            props.Add(new LevelMethodDescriptor("Name", del, typeof(string)));

            del = delegate(Level l)
            {
                return l.players.Count;
            };
            props.Add(new LevelMethodDescriptor("Players", del, typeof(int)));

            del = delegate(Level l)
            {
                return l.physics;
            };
            props.Add(new LevelMethodDescriptor("Physics", del, typeof(int)));

            del = delegate(Level l)
            {
                //return l.permissionvisit.ToString();
                return Group.GroupList.Find(grp => grp.Permission == l.permissionvisit).name;
            };
            props.Add(new LevelMethodDescriptor("PerVisit", del, typeof(string)));

            del = delegate(Level l)
            {
                //return l.permissionbuild.ToString();
                return Group.GroupList.Find(grp => grp.Permission == l.permissionbuild).name;
            };
            props.Add(new LevelMethodDescriptor("PerBuild", del, typeof(string)));

            PropertyDescriptor[] propArray = new PropertyDescriptor[props.Count];
            props.CopyTo(propArray);
            return new PropertyDescriptorCollection(propArray);
        }
    }

    public delegate object LevelMethodDelegate(Level l);

    public class LevelMethodDescriptor : PropertyDescriptor
    {
        protected LevelMethodDelegate _method;
        protected Type _methodReturnType;

        public LevelMethodDescriptor(string name, LevelMethodDelegate method,
         Type methodReturnType)
            : base(name, null)
        {
            _method = method;
            _methodReturnType = methodReturnType;
        }

        public override object GetValue(object component)
        {
            Level l = (Level)component;
            return _method(l);
        }

        public override Type ComponentType
        {
            get { return typeof(Level); }
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
