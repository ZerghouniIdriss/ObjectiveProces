 

namespace OkrsEntreprise.Model.Entities
{ 

        public interface IEntity<T> where T : struct
        { 
            T Id { get; }
        }
    
}
