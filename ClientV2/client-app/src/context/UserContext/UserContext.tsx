import { createContext } from "react";
import { USER_ACTIONS } from "../../actions/UserActions/UserActions";
import { UserType } from "../../types/userType";

export type UserContextPropsType = {
  state: UserType,
  dispatch: React.Dispatch<{type: USER_ACTIONS, payload: UserType}>
}

export const UserContext = createContext<UserContextPropsType | undefined>(undefined)