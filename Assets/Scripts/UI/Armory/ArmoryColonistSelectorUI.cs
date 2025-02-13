using System.Collections.Generic;
using UnityEngine;

public class ArmoryColonistSelectorUI : MonoBehaviour
{
   [SerializeField] private Transform selectorContainerUI;
   [SerializeField] private Transform armoryRosterUIButtonPrefab;
   [SerializeField] private Transform selectorButtonsContainerUI;

   private CharacterManager _characterManager;
   private ArmorySlotSelector _armorySlotSelector;
   
   private void Start()
   {
      _armorySlotSelector = FindObjectOfType<ArmorySlotSelector>();
      _armorySlotSelector.OnSelectedArmorySlotChanged += OnSelectedArmorySlotChanged;
      selectorContainerUI.gameObject.SetActive(false);
      _characterManager = FindObjectOfType<CharacterManager>();
      _characterManager.OnCharacterDataListRestored += OnCharacterDataListRestored;
   }

   private void OnSelectedArmorySlotChanged(ArmorySlot armorySlot)
   {
      if (armorySlot)
      {
         selectorContainerUI.gameObject.SetActive(true);
         RefreshUI(_characterManager.GetCharacterDataList()); 
         return;
      }
      selectorContainerUI.gameObject.SetActive(false);
   }

   private void OnCharacterDataListRestored(List<CharacterData> characterDataList)
   {
      RefreshUI(characterDataList);
   }
   
   private void RefreshUI(List<CharacterData> characterDataList)
   {
      foreach (Transform buttonTransform in selectorButtonsContainerUI)
      {
         Destroy(buttonTransform.gameObject);
      }

      foreach (CharacterData characterData in characterDataList)
      {
         Transform characterButtonTransform = Instantiate(armoryRosterUIButtonPrefab, selectorButtonsContainerUI);
         ArmoryColonistSelectButton armoryColonistSelectButton = characterButtonTransform.GetComponent<ArmoryColonistSelectButton>();
         armoryColonistSelectButton.SetCharacterData(characterData);
      }
   }
}
