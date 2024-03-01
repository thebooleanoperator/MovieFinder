import { createContext, useContext } from "react"
import { ApiContext } from "../CoreApiContext/CoreApiContext"
import { LoginService } from "../../api/LoginApi/LoginApi"

export const LoginApiContext = createContext<LoginService | undefined>(undefined);

export const LoginApiProvider = ({children}: any) => {
  const apiService = useContext(ApiContext)
  const loginService = new LoginService(apiService)

  return (
    <LoginApiContext.Provider value={loginService}>
      {children}
    </LoginApiContext.Provider>
  )
}