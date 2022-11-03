import React from 'react';
import { useEffect, useState } from 'react';
import { Navigate } from 'react-router';
import { useNavigate } from 'react-router-dom';
import { requestInfo  } from '../services/requests';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [button, setButton] = useState(true);
  const [isloginSuced, setIsloginSuced] = useState(false);
  const [isLoginValid, setIsLoginValid] = useState(true);
  const history = useNavigate();

  async function handleLoginButton(e) {
    e.preventDefault();
    try {
      const req = await requestInfo({ email, password });
      localStorage.setItem('user', JSON.stringify(req));
      setIsloginSuced(true);
    } catch (error) {
      return setIsLoginValid(false);
    }
  }

  useEffect(() => {
    function validateNavigate() {
      if (localStorage.getItem('user')) {
        // history.push('/feed');
      }
    }

    function validateInputs() {
      const regexEmail = /^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/;
      const MIN_PASSWORD = 8;
      if (
        email.match(regexEmail)
        && parseFloat(password.length) >= MIN_PASSWORD
      ) {
        return setButton(false);
      }
      return setButton(true);
    }

    validateNavigate();
    validateInputs();
  }, [password, email, history]);

  if (isloginSuced) {
      return <Navigate replace to="/feed" />;
  }

  return (
    <div>
      <h1>TRYITTER</h1>
      <p>logo</p>
      <form>
        <label htmlFor="username">
          <p>Username</p>
          <input
            type="text"
            value={ email }
            onChange={ ({ target: { value } }) => setEmail(value) }
          />
        </label>
        <label htmlFor="Password">
          <p>Password</p>
          <input
            type="password"
            value={ password }
            onChange={ ({ target: { value } }) => setPassword(value) }
          />
        </label>
        <div>
          <button
            type="submit"
            disabled={ button }
            onClick={ handleLoginButton }
          >
            login
          </button>
        </div>
        <div>
          <button
            type="button"
            onClick={ () => history.push('/register') }
          >
            Criar conta
          </button>
        </div>
      </form>
      {isLoginValid
        ? null : (
          <p data-testid="common_login__element-invalid-email">
            Endereço de e-mail ou senha inválidos, tente novamente.
          </p>
        ) }
    </div>
  );
}
