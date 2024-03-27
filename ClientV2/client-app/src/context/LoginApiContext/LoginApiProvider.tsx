import { useContext } from "react"
import { LoginApiContext } from "./LoginApiContext"
import { LoginService } from "../../api/LoginApi/LoginApi"
import { ApiContext } from "../CoreApiContext/CoreApiContext"

type LoginApiProviderProps = {
  children: React.ReactNode;
}

export const LoginApiProvider = ({children}: LoginApiProviderProps) => {
  const apiService = useContext(ApiContext)
  const loginService = new LoginService(apiService)

  return (
    <LoginApiContext.Provider value={loginService}>
      {children}
    </LoginApiContext.Provider>
  )
}