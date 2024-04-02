import { useReducer } from "react"
import { UserContext } from "./UserContext"
import { UserReducer } from "../../reducers/UserReducer"

type UserProviderProps = {
  children: React.ReactNode;
}

export const UserProvider: React.FC<UserProviderProps> = ({children}) => {
  const defaultUser = { jwt: '', userId: '' }
  const [state, dispatch] = useReducer(UserReducer, defaultUser)

  return (
    <UserContext.Provider value={{state, dispatch}}>
      {children}
    </UserContext.Provider>
  )
}