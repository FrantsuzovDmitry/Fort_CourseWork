using Assets.Scripts.Cards;

namespace Assets.Scripts.Managers.MediatorModules
{
    public class UserActionsValidator
    {
        public string LastErrorMessage => SelectMessageToUser();

        public bool ValidateUserAction(GroupOfCharacters groupOfCharacters, Fortress selectedFortToAttack)
        {
            if (groupOfCharacters.CardInGroup < 1)
            {
                validationResult = UserActionValidationResult.NoAttackersSelected;
                return false;
            }

            bool goupIsValid = selectedFortToAttack.ValidateAttackersGroup(groupOfCharacters);
            if (!goupIsValid)
            {
                validationResult = UserActionValidationResult.InvalidAttackersGroup;
                return false;
            }

            validationResult = UserActionValidationResult.Success;
            return true;
        }

        private string SelectMessageToUser()
        {
            switch (validationResult)
            {
                case UserActionValidationResult.Success:
                    return string.Empty;
                case UserActionValidationResult.NoAttackersSelected:
                    return "Выберите хотя бы одного атакующего!";
                case UserActionValidationResult.InvalidAttackersGroup:
                    return "Некорректный отряд защитников";
                default:
                    return string.Empty;
            }
        }

        private enum UserActionValidationResult
        {
            Success,
            NoAttackersSelected,
            InvalidAttackersGroup
        }

        private UserActionValidationResult validationResult;
    }
}
