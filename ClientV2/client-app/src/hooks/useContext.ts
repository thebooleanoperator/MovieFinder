import { useContext } from "react";

export const useSafeContext = <T>(context: React.Context<T | undefined>) => {
  const foundContext = useContext(context); 

  if (!foundContext) {
    // Handle the case where context is not available
    throw new Error('UserContext must be used within a UserContextProvider');
  }
  
  return foundContext
}