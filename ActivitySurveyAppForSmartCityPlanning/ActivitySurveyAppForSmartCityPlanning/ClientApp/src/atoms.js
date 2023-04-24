import { atom } from "recoil";

// for dark theme
export const isDarkAtom = atom({
  key: "isDark",
  default: false,
});

// to store user token
export const authAtom = atom({
  key: "auth",
  // get initial state from local storage to enable user to stay logged in
  default: JSON.parse(localStorage.getItem("user")),
});
