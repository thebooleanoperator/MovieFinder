import { BottomNavigation, BottomNavigationAction } from "@mui/material"
import { LOGIN, REGISTER } from "../router/routeConstants"
import { AppRegistration, Login as MuiLogin } from "@mui/icons-material"

export const MainLayout =({children}: any) =>{
  // get isLoggedIn from context
  
  return(
      <>
        <main>{children}</main>
        <BottomNavigation showLabels>
          <BottomNavigationAction label="Register" href={REGISTER} icon={<AppRegistration />} />
          <BottomNavigationAction label="Login" href={LOGIN} icon={<MuiLogin />} />
        </BottomNavigation>
      </>
  )
}