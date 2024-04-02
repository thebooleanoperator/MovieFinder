import { USER_ACTIONS, UserActionsType } from "../actions/UserActions/UserActions";
import { UserType } from "../types/userType";

export const UserReducer = (state: UserType, action: UserActionsType) => {
  switch (action.type) {
    case USER_ACTIONS.UPDATE_JWT:
      return action.payload
    default:
      return state
  }
}