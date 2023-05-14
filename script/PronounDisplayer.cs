using XRL.UI;
using System.Text;

namespace XRL.World.Parts {
  // Most objects are assigned pronouns by their gender.
  // Some have special extra pronouns on top of that.
  // Granted, "genders" includes a lot of nonbinary/xenogenders, so this implies
  // you can be trans-transgender in CoQ
  public class PKPRN_PronounDisplayer : IPart {
    public static readonly string KEY_PRONOUNS = "getpronouns";

    public override bool WantEvent(int id, int cascade) {
      return
             id == GetShortDescriptionEvent.ID
          || id == GetInventoryActionsEvent.ID
          || id == InventoryActionEvent.ID
          || base.WantEvent(id, cascade);
    }

    public override bool HandleEvent(GetShortDescriptionEvent e) {
      if (ParentObject != null && !ParentObject.IsPlayer()) {
        if (ParentObject.GetPronounSetIfKnown() != null) {
          e.Postfix.Append("\nYou know ");
          e.Postfix.Append(ParentObject.its);
          e.Postfix.Append(" pronouns to be ");
          e.Postfix.Append(ParentObject.GetPronounProvider().Name);
          e.Postfix.Append(".\n");
        }
        // Could do something with guessing its pronouns from its gender, but that feels rude
      }

      return base.HandleEvent(e);
    }

    public override bool HandleEvent(GetInventoryActionsEvent e) {
      // Don't check for if you know the pronouns here so you can learn the pronouns of hostile beings
      e.AddAction("Show Pronouns", "show pronouns", KEY_PRONOUNS, Key: 'P', WorksAtDistance: true);
      return base.HandleEvent(e);
    }

    public override bool HandleEvent(InventoryActionEvent e) {
      if (e.Command == KEY_PRONOUNS) {
        var bob = new StringBuilder();
        bob.Append("Pronoun set: ");
        bob.Append(ParentObject.GetPronounProvider().Name);
        bob.Append("\n");
        bob.Append(ParentObject.GetPronounSet()?.GetSummary()
          ?? ParentObject.GetGender().GetSummary());
        Popup.Show(bob.ToString(), LogMessage: false);
      }
      return base.HandleEvent(e);
    }
  }
}
