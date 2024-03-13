import { LoginService } from "../../../api/LoginApi/LoginApi"
import { useContext } from "react"
import { LoginApiContext } from "../../../context/LoginApiContext/LoginApiContext"
import { Form} from 'react-final-form'
import { Button, Card, CardActions, CardContent, Typography } from "@mui/material"
import { MuiTextField } from "../../UI/MuiTextField"
import { required } from "../../../validators/fieldValidators"

type LoginFormState = {
  username: string,
  password: string
}

type LoginFormProps = {
  handleError: (errorMessage: string) => void
}

export const LoginForm: React.FC<LoginFormProps> = ({ handleError }: LoginFormProps) => {
  const loginService = useContext(LoginApiContext) as LoginService

  const handleLogin = async (values: LoginFormState) => {
    const response = await loginService.Login(values.username, values.password)
    console.log(response)
    if (!validateResponse(response)) {
      handleError('Sign In Failed')
    }
  }

  const handleGuestLogin = async () => {
    const response = await loginService.LoginAsGuest()
    console.log(response)
    if (!validateResponse(response)) {
      handleError('Guest Login Failed')
    }
  }

  // TODO define service request / response
  const validateResponse = (response: any): boolean => {
    if (!response) {
      return false
    }
    return true
  }

  const handleFormValidation = (values: LoginFormState) => {
    const errors: {[key: string]: string | undefined} = {}
    errors.username = required(values.username)
    errors.password = required(values.password)
    return errors
  }

  return (
    <Card variant="outlined" sx={{maxWidth: "50%"}}>
      <CardContent>
        <Typography gutterBottom variant="h4" component="div">
          Sign In
        </Typography>
        <Form
          onSubmit={handleLogin}
          validate={handleFormValidation}
          render={({handleSubmit}) => (
          <form onSubmit={handleSubmit}>
            <div style={{display: "flex", flexDirection:"column", maxWidth: "75%"}}>
              <MuiTextField name="username" label="username" />
              <MuiTextField name="password" label="password" />
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