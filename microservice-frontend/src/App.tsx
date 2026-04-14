import { useState } from 'react'

import './App.css'
import { Link, Route, Routes } from 'react-router'
import { Layout, Menu } from 'antd'
import { Content, Footer, Header } from 'antd/es/layout/layout'
import { signOut, store } from './Store'
import { PrivateRoute } from './Components/PrivateRoute'
import LoginSection from './Components/LoginSection'
import RegisterSection from './Components/RegisterSection'
import ProfileSection from './Components/ProfileSection'
import ProfilesSection from './Components/ProfilesSection'
import NotificationsRuleSection from './Components/NotificationRuleSection'

function App() {
  const [isAuthenticated, setAuth] = useState<boolean>(store.getState().Auth.isAuthenticated)
  const [Login, setLogin] = useState(store.getState().Auth.login)
  function ExecuteLogout(): void {
    document.cookie = "Authtoken=; Max-Age=-1;"
    store.dispatch(signOut())
  }
  store.subscribe(() => {
    let authSlice = store.getState().Auth
    setAuth(authSlice.isAuthenticated)
    setLogin(authSlice.login)
  })
  return (
    <>
      <Layout style={{ minHeight: "100vh" }}>
        <Header>
          <Menu theme="dark"
            mode="horizontal"
            items={isAuthenticated ? [
              { key: "siteName", label: <Link to={"/"}>Минималичтичная соцсеть</Link> },
              { key: "profile", label: <Link to={"/Profile"}>Профиль</Link> },
              { key: "tasks", label: <Link to={"/Profiles"}>Поиск профилей</Link> },
              { key: "norifications", label: <Link to={"/NotificationSettings"}>Настройка уведомлений</Link> },
              { key: "logout", label: <input type="button" onClick={ExecuteLogout} value="Выход" /> },
            ] : [
              { key: "siteName", label: <Link to={"/"}>Минималичтичная соцсеть</Link> },
              { key: "login", label: <Link to={"/Login"}>Вход</Link> },
              { key: "registry", label: <Link to={"/Registry"}>Регистрация</Link> },

            ]}
            style={{ flex: 1, minWidth: 0 }}
          ></Menu>
        </Header>
        <Content style={{ padding: "0 48px" }}>
          <Routes>
            <Route path="/Login" element={<LoginSection />} />
            <Route path="/Registry" element={<RegisterSection />} />
            <Route element={<PrivateRoute />}>
              <Route path='/Profile' element={<ProfileSection />} />
              <Route path='/Profile/:login' element={<ProfileSection />} />
              <Route path='/Profiles' element={<ProfilesSection />} />
              <Route path='/NotificationSettings' element={<NotificationsRuleSection />} />
            </Route>
          </Routes>
        </Content>
        <Footer style={{ textAlign: "center" }}>
          Math Battles 2025 Created by me
        </Footer>
      </Layout>
    </>
  )
}

export default App
