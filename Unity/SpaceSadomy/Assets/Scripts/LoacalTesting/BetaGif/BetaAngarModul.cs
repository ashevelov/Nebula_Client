using UnityEngine;
using System.Collections;
using Common;

    public class BetaAngarModul : MonoBehaviour
    {
        public ShipModelSlotType slotType;
        private BetaAngarModulController _modulController;

        void OnMouseEnter()
        {
            this.GetComponent<Renderer>().material.SetFloat("_EmissionLM", 0.6f);
        }

        void OnMouseExit()
        {
            this.GetComponent<Renderer>().material.SetFloat("_EmissionLM", 0.0f);
        }

        void OnMouseDown()
        {
            if (_modulController == null)
            {
                _modulController = FindObjectOfType<BetaAngarModulController>();
            }
            _modulController.SwitchModule(slotType);
        }
    }
