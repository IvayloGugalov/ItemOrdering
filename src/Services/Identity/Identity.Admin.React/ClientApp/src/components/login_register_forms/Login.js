import React, { useRef, useState, useEffect } from 'react'
import { useNavigate, useLocation } from 'react-router-dom';
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { variables } from '../../Variables';
import useAuth from '../../custom-hooks/useAuth';
import useLocalStorage from '../../custom-hooks/useLocalStorage';
import useToggle from '../../custom-hooks/useToggle';
import useAxiosPrivate from '../../custom-hooks/useAxiosPrivate';

const Login = () => {
  const { setAuth } = useAuth();
  const axiosPrivate = useAxiosPrivate();

  const navigate = useNavigate();
  const location = useLocation();
  const from = location.state?.from?.pathName || '/';

  const errorRef = useRef();

  // const [email, setEmail] = useState('');
  const [email, setEmail] = useLocalStorage('email', '');
  const [password, setPassword] = useState('');
  const [checkRememberMe, toggleRememberMe] = useToggle('rememberMe', false);
  const [errorMsg, setErrorMsg] = useState('');

  useEffect(() => {
    setErrorMsg('');
  }, [email, password]);

  const handleLogin = async (e) => {
    e.preventDefault();

    try {
      const response = await axiosPrivate.post(variables.IDENTITY_API_URL + "login",
        JSON.stringify({ email, password }),
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        }
      );

      console.log(JSON.stringify(response?.data));
      const accessToken = response?.data?.accessToken;
      const roles = response?.data?.roles?.split('');

      setAuth({ email, password, roles, accessToken });
      navigate('/home', { replace : true });
    } catch (err) {
      if (!err.response) {
          setErrorMsg(`Error, no response ${err}`);
      } else {
          setErrorMsg('Registration Failed');
      }
      errorRef.current.focus();
    }
  };

  return (
    <article className='justify-content-md-center col-md-6'>
      <Form onSubmit={handleLogin} >
        <h3 className='d-flex justify-content-center my-4' >
          Sign In
        </h3>
        <Form.Group className="mb-3" controlId="formBasicEmail">
          <Form.Label>Email address</Form.Label>
          <Form.Control type="email" placeholder="Enter email" onChange={(e) => setEmail(e.target.value)} />
        </Form.Group>
        <Form.Group className="mb-3" controlId="formBasicPassword">
          <Form.Label>Password</Form.Label>
          <Form.Control type="password" placeholder="Enter password" onChange={(e) => setPassword(e.target.value)} />
        </Form.Group>
        <Row>
          <Col>
            <Form.Group className="mb-3" controlId="formBasicCheckbox">
              <Form.Check type="checkbox" label="Remember me"
               onChange={toggleRememberMe} checked={checkRememberMe} />
            </Form.Group>
          </Col>
          <Col>
            <p className="forgot-password text-right">
              <a href="#">Forgot password?</a>
            </p>
          </Col>
        </Row>
        <Row>
          <Col className='d-flex justify-content-center'>
            <Button variant="primary" type="submit" className='w-50'>
              Log in
            </Button>
          </Col>
        </Row>
      </Form>

      <Row className='d-flex justify-content-center mt-4'>
        <p>
          Don't have an account?
          <a href='/register'> Sing up</a>
        </p>
      </Row>
      <p ref={errorRef} className={errorMsg
             ? "errmsg"
             : "offscreen"}>
                 {errorMsg}
             </p>
    </article>
  );
}

export default Login;