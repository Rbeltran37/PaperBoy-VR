using System.Collections.Generic;
using UnityEngine;

namespace Core.Persistence
{
    public abstract class UnitOfWork : ScriptableObject
    {
        [SerializeField] private DataContext dataContext;
        

        public async void Save() => await dataContext.Save();
    }
}
