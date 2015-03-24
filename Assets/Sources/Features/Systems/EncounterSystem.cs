using Entitas;
using UnityEngine;

public class EncounterSystem : IExecuteSystem, ISetPool
{
    Pool _repo;
    Group _collection;

    public void SetPool(Pool repo)
    {
        _repo = repo;
        //_collection = _repo.GetGroup(Matcher.AllOf(Matcher.EncounterTrigger, Matcher.OnTriggerEnterRef));
        _collection = _repo.GetGroup(Matcher.AllOf(Matcher.Resource, Matcher.Resource));

    }

    public void Execute()
    {
       if (_collection.Count != 0)
        {

            //var myTag = _repo.encounterMonoRef.trigger.tag;
            var myTag = "butt";
            var compareTags = _repo.encounterTrigger.resourceTrigger;
            for (int i = 0; i < compareTags.Length; i++)
            {
                if (myTag == compareTags[i])
                {
                    _repo.encounterTrigger.resourceTrigger[i] = "";
                    Debug.Log("I work I work");
                    
                }
            }
        }

    }

}