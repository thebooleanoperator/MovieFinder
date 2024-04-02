import { useContext } from "react"
import { LoginApiContext } from "../../../context/LoginApiContext/LoginApiContext"
import { Form} from 'react-final-form'
import {
  Button, 
  Card,
  CardActions,
  CardContent,
  Typography } from "@mui/material"
import { MuiTextField } from "../../UI/MuiTextField"
import { required } from "../../../validators/fieldValidators"
import { UserContext } from "../../../context/UserContext/UserContext"
import { USER_ACTIONS } from "../../../actions/UserActions/UserActions"
import { LoginResponseType } from "../../../types/apiType"
import { useSafeContext } from "../../../hooks/useContext"

type LoginFormState = {
  email: string,
  password: string
}

type LoginFormProps = {
  handleError: (errorMessage: string) => void
}

export const LoginForm: React.FC<LoginFormProps> = ({ handleError }) => {
  const loginService = useContext(LoginApiContext)
  const { state: user, dispatch: dispatchUser } = useSafeContext(UserContext)

  const handleLogin = async (values: LoginFormState) => {
    const response = await loginService.Login(values.email, values.password)
    console.log(response)
    if (!validateLoginResponse(response)) {
      handleError('Sign In Failed')
    }
    user.jwt = response?.value?.jwt ?? ''
    dispatchUser({type: USER_ACTIONS.UPDATE_JWT, payload: user})
  }

  const handleGuestLogin = async () => {
    const response = await loginService.LoginAsGuest()
    console.log(response)
    if (!validateLoginResponse(response)) {
      handleError('Guest Login Failed')
    }
  }

  const validateLoginResponse = (response: LoginResponseType) => {
    if (!response.isSuccess) {
      return false
    }
    return !!response?.value?.jwt
  }

  return (
    <Card variant="outlined" sx={{maxWidth: "50%"}}>
      <CardContent>
        <Typography gutterBottom variant="h4" component="div">
          Sign In
        </Typography>
        <Form
          onSubmit={handleLogin}
          render={({handleSubmit}) => (
          <form onSubmit={handleSubmit}>
            <div style={{display: "flex", flexDirection:"column", maxWidth: "75%"}}>
              <MuiTextField name="email" label="email" validate={required} />
              <MuiTextField name="password" label="password" validate={required} />
            </div>
            <CardActions>
              <Button type="submit" variant="contained">Sign In</Button>
              <Button type="button" variant="contained" onClick={async () => {await handleGuestLogin()}}>Guest Login</Button>
            </CardActions>
          </form>
          )}>
        </Form>
      </CardContent>
    </Card>
  )
}