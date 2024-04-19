using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


namespace Bookkeeping
{
    [Serializable]
    public class Entry : ISerializationCallbackReceiver
    {
        public string name;
        public float amount;
        public Category category;
        public string dateString;
        public DateTime date;
        public Person sharedWith;
        public float owedPercentage;
        public bool owedByMe; //Si oui, c'est moi qui doit de l'argent à cette personne ; si non c'est elle qui me doit de l'argent
        public bool repaid;

        public bool Shared => sharedWith != null && !string.IsNullOrEmpty(sharedWith.name);

        public float FinalAmount
        {
            get
            {
                if(Shared)
                {
                    if(owedByMe)
                    {
                        //Si je dois 40% d'une dépense de 50€, ça me coûte 20€
                        return amount * owedPercentage;
                    }
                    else
                    {
                        //Si on me doit 30% d'une dépense de 50€, ça me coûte (1-30%) = 70% * 50€ = 35€
                        return (1 - owedPercentage) * amount;
                    }
                }
                else
                {
                    return amount;
                }
            }
        }
        
        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            ParseDate();
        }

        public void SetDate(string dateString)
        {
            this.dateString = dateString;
            ParseDate();
        }

        private void ParseDate()
        {
            try
            {
                date = DateTime.Parse(dateString, CultureInfo.CreateSpecificCulture("fr-FR"));
            }
            catch (FormatException)
            {
                Debug.Log("Cannot parse data to DateTime : " + dateString+" ("+name+", "+amount+", "+FinalAmount+")");
            }
        }

        public float DebtAmount => repaid ? 0 : owedPercentage * amount * (owedByMe ? 1 : -1);
    }
}