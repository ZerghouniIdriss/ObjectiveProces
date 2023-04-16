using OkrsEntreprise.DAL.Context;
using OkrsEntreprise.Model.Entities;
using System.Linq;

namespace OkrsEntreprise.DAL.Repositories
{
    public interface IKeyResultRepository : IRepositoryBase<Model.Entities.KeyResult>
    {
        Goal AddKeyResultToGoal(Goal goalToUpdate, KeyResult keyResult);

        KeyResult EditKeyResult(KeyResult keyResult);

        KeyResult DeleteKeyResult(KeyResult keyResultToDelete);

    }

    public class KeyResultRepository : RepositoryBase<Model.Entities.KeyResult>, IKeyResultRepository
    {
        public Goal AddKeyResultToGoal(Goal goalToUpdate, KeyResult keyResult)
        {
            using (var context = new OkrsContext())
            {
                var goal = context.Goals.FirstOrDefault(x => x.Id == goalToUpdate.Id);

                if (goal != null)
                {
                    goal.KeyResults.Add(keyResult);

                    context.KeyResults.Add(keyResult);

                    SaveContextChange(context);
                }

                return goal;
            }
        }

        public KeyResult EditKeyResult(KeyResult keyResultToUpdate)
        {
            using (var context = new OkrsContext())
            {
                var keyResult = context.KeyResults.FirstOrDefault(x => x.Id == keyResultToUpdate.Id);

                if (keyResult != null)
                {
                    keyResult.Title = keyResultToUpdate.Title;
                    SaveContextChange(context);
                }

                return keyResult;
            }
        }


        public KeyResult DeleteKeyResult(KeyResult keyResultToDelete)
        {
            using (var context = new OkrsContext())
            {
                var keyResult = context.KeyResults.FirstOrDefault(x => x.Id == keyResultToDelete.Id);

                if (keyResult != null)
                {
                    context.KeyResults.Remove(keyResult);
                    SaveContextChange(context);
                }
                return keyResult;
            }
        }

    }
}
