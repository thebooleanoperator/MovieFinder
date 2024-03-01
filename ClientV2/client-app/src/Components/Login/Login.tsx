import { Button } from "@mui/material"
import { LoginProps } from "../../api/LoginApi/LoginApi"
import { useContext } from "react"
import { LoginApiContext } from "../../context/LoginApiContext/LoginApiContext"

export const Login: React.FC = () => {
  const loginService = useContext(LoginApiContext)

  const handleLogin = async () => {
    const response = await loginService?.Login('test', '123')
    console.log(response)
  }
  
  return (
    <>
      <div>Hello Nerd</div>
      <Button variant="outlined" onClick={async () => await handleLogin()}>Test Login</Button>
    </>
  )
}