using System;
using System.Collections.Generic;
using System.Text;
using NoteController.Interfaces;
using Models;
using Models.ServerModel;
using System.Linq;
using System.Collections.ObjectModel;

namespace NoteController.Controller
{
    public class Notes_Controller : IEntityCollectionController<Note, ObservableCollection<Note>, Notes>
    {
        /// <summary>
        /// Adds Entity to Entity collection
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="EntityCollection"></param>
        public void AddEntity(Note Entity, ObservableCollection<Note> EntityCollection)
        {
            EntityCollection.Add(Entity);
        }
        
        /// <summary>
        /// Marks Entities that should be removed from Db
        /// </summary>
        /// <param name="index"></param>
        /// <param name="EntityCollection"></param>
        public void RemoveEntity(int index, ObservableCollection<Note> EntityCollection)
        {
            EntityCollection[index].Remove = true;
        }
    }
}
