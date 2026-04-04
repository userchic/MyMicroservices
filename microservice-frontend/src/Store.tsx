import { createSlice, configureStore } from "@reduxjs/toolkit";

let authFlagString = localStorage.getItem("authFlag")
let authFlag = localStorage.getItem("authFlag") === "true" ? true : false


const authSlice = createSlice({
    name: 'authentication',
    initialState: {
        isAuthenticated: localStorage.getItem("authFlag") === null ? false : localStorage.getItem("authFlag") === "true" ? true : false,
        login: localStorage.getItem("login") === null ? "" : localStorage.getItem("login")
    },
    reducers: {
        signIn: (state, login) => {
            state.isAuthenticated = true;
            state.login = login.payload
            localStorage.setItem("authFlag", "true")
            localStorage.setItem("login", login.payload)
        },
        signOut: (state) => {
            state.isAuthenticated = false;
            state.login = ""
            localStorage.setItem("authFlag", "false")
            localStorage.setItem("login", "")
        },
    },
});

// Экшены
export const { signIn, signOut } = authSlice.actions;

// Store
export const store = configureStore({
    reducer: {
        Auth: authSlice.reducer,
    },
});