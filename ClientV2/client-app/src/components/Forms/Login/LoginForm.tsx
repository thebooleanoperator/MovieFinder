import { LoginProps } from "../../../api/LoginApi/LoginApi"
import { useContext } from "react"
import { LoginApiContext } from "../../../context/LoginApiContext/LoginApiContext"
import { Form, Field } from 'react-final-form'
import { Button, Card, CardActionArea, CardActions, CardContent, CardHeader, Typography } from "@mui/material"
import { MuiTextField } from "../../UI/MuiTextField"

export const LoginForm: React.FC = () => {
  const loginService = useContext(LoginApiContext)

  const handleSubmit = async () => {
    const response = await loginService?.Login('test', '123')
    console.log(response)
  }

  const handleGuestLogin = async () => {
    const response = await loginService?.LoginAsGuest()
    console.log(response)
  }

  const validateLogin = () => {

  }
  
  return (
    <Card variant="outlined" sx={{maxWidth: "50%"}}>
      <CardContent>
        <Typography gutterBottom variant="h4" component="div">
          Sign In
        </Typography>
        <Form
          onSubmit={handleSubmit}
          render={({handleSubmit}) => (
          <form onSubmit={handleSubmit}>
            <div style={{display: "flex", flexDirection:"column", maxWidth: "75%"}}>
              <MuiTextField name="username" label="username" />
              <MuiTextField name="password" label="password" />
            </div>
            <CardActions>
              <Button type="submit" variant="contained">Submit</Button>
              <Button type="button" variant="contained" onClick={async () => await handleGuestLogin()}>Guest Login</Button>
            </CardActions>
          </form>
          )}>
        </Form>
      </CardContent>
    </Card>
  )
}