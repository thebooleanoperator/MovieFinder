import { AppBar, Box, Button, IconButton, Toolbar, Typography } from "@mui/material"
import { useEffect, useState } from "react"
import { useLocation, useNavigate } from "react-router-dom"
import { LOGIN, REGISTER } from "../../constants/routeConstants"

export const MuiAuthAppBar = () => {
  const location = useLocation()
  const navigation = useNavigate()
  const [onLoginPage, setOnLoginPage ] = useState(false)

  useEffect(() => {
    setOnLoginPage(location.pathname === LOGIN)
  }, [location])

  const handleNavigation = () => {
    const route = onLoginPage ? REGISTER : LOGIN
    navigation(route)
  }

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static">
        <Toolbar>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="menu"
            sx={{ mr: 2 }}
          >
          </IconButton>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Movie Presto
          </Typography>
          {
            onLoginPage 
              ? <Button color="inherit" onClick={handleNavigation}>Register</Button> 
              : <Button color="inherit" onClick={handleNavigation}>Login</Button>
          }
        </Toolbar>
      </AppBar>
    </Box>
  )
}