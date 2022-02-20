using System;
using System.Collections.Generic;
using System.Text;

namespace NoteController.Interfaces
{
    public interface IEntityCollectionController<T, TCol, Tout>
        where T: class        
    {
        void AddEntity(T Entity, TCol EntityCollection);

        void RemoveEntity(int index, TCol EntityCollection);                
    }
}
