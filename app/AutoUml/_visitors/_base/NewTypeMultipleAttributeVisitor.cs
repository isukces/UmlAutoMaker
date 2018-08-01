using System;
using System.Reflection;
using JetBrains.Annotations;

namespace AutoUml
{
    public abstract class NewTypeMultipleAttributeVisitor<T> : INewTypeInDiagramVisitor
        where T : Attribute
    {
        public void Visit(UmlProjectDiagram diagram, UmlEntity info)
        {
            
            var t = info.Type;
            foreach (var att in t.GetCustomAttributes<T>())
                VisitInternal(diagram, info, att);
        }

        protected abstract void VisitInternal(UmlProjectDiagram diagram, UmlEntity info, [NotNull] T att);
    }
}