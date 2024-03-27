import { ReactNode } from "react";
import { LoginApiProvider } from "./LoginApiContext/LoginApiProvider"
import { RegisterApiProvider } from "./RegisterApiContext/RegisterApiProvider"

type RootProviderProps = {
  children: ReactNode;
};

export const RootProvider: React.FC<RootProviderProps> = ({children}) => (
  <LoginApiProvider>
    <RegisterApiProvider>
      {children}
    </RegisterApiProvider>
  </LoginApiProvider>
)