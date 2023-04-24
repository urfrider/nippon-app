import { Navigate } from "react-router-dom";
import { useRecoilValue } from "recoil";
import { authAtom } from "../atoms";

function PrivateRoute({ children }) {
  const auth = useRecoilValue(authAtom);
  // check if authenticated
  if (!auth) {
    console.log("not authorised");
    // not logged in so redirect to login page with the return url
    return <Navigate to="/" />;
  }
  return children;
}

export default PrivateRoute;
