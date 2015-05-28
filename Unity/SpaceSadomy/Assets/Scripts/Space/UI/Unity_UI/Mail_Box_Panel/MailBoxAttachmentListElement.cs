using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Nebula.Client.Mail;
using Common;
using Game.Space;
using Nebula.Client.Inventory;
using Nebula.Client;

public class MailBoxAttachmentListElement : MonoBehaviour {

    public Image attachmentIconImage;
    public UnityEngine.UI.Button elementButton;
    public Text attachmentCountText;
    public Image colorImage;

    private ClientAttachment attachment;

    public void SetAttachment(ClientAttachment attachment) {
        this.attachment = attachment;
        int count = 0;
        switch (attachment.Type()) {
            case AttachmentType.InventoryObject: {
                ClientInventoryObjectAttachment inventoryObjectAttachment = attachment as ClientInventoryObjectAttachment;
                attachmentIconImage.sprite = SpriteCache.SpriteForItem(inventoryObjectAttachment.AttachedObject());
                count = inventoryObjectAttachment.Count();
                SetColorImage(inventoryObjectAttachment.AttachedObject());             
                }
                break;
            case AttachmentType.ShipModule: {
                ClientShipModuleAttachment shipModuleAttachment = attachment as ClientShipModuleAttachment;
                attachmentIconImage.sprite = SpriteCache.SpriteModule(shipModuleAttachment.AttachedObject().templateId);
                count = shipModuleAttachment.Count();
                SetColorImage(shipModuleAttachment.AttachedObject());
                }
                break;
            default: {
                throw new NebulaException("Unknown attachment type: {0}".f(attachment.Type()));
                }
        }

        attachmentCountText.text = count.ToString();
    }

    private void SetColorImage(object itObject) {
        if (itObject is IInventoryObjectInfo) {
            var obj = itObject as IInventoryObjectInfo;
            if (obj.HasColor()) {
                colorImage.sprite = SpriteCache.SpriteColor(obj);
                return;
            } 
        } else if (itObject is ClientShipModule) {
            colorImage.sprite = SpriteCache.SpriteColor(itObject as ClientShipModule);
            return;
        }
        colorImage.gameObject.SetActive(false);
    }

    public ClientAttachment Attachment() {
        return this.attachment;
    }
}
