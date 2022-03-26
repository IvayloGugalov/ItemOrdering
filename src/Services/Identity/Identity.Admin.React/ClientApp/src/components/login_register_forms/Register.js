import React, { useRef, useState, useEffect } from "react";
import axios from "../../api/axios";
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button'
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { variables } from "../../Variables";

const Register = () => {
    const errorRef = useRef();

    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [email, setEmail] = useState('');
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const [errorMsg, setErrorMsg] = useState('');

    const handleRegister = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post(variables.IDENTITY_API_URL + '/register',
                JSON.stringify({ firstName, lastName, email, userName, password }),
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
        } catch (err) {
            if (!err.response) {
                setErrorMsg('Error, no response');
            } else {
                setErrorMsg('Registration Failed');
            }
            errorRef.current.focus();
        }
    }

    useEffect(() => {
        setErrorMsg('');
    }, [firstName, lastName, email, userName, password]);

    return (
        <section>
            <h3>Sign Up</h3>
            <Form className='justify-content-md-center col-md-6' onSubmit={handleRegister} >
                <h3 className='d-flex justify-content-center my-4' >
                    Sign Up
                </h3>
                <Form.Group className="mb-3" controlId="formBasicFirstName">
                    <Form.Label>Email address</Form.Label>
                    <Form.Control type="text" placeholder="Enter first name" onChange={(e) => setFirstName(e.target.value)} />
                </Form.Group>
                <Form.Group className="mb-3" controlId="formBasicLastName">
                    <Form.Label>Email address</Form.Label>
                    <Form.Control type="text" placeholder="Enter last name" onChange={(e) => setLastName(e.target.value)} />
                </Form.Group>
                <Form.Group className="mb-3" controlId="formBasicEmail">
                    <Form.Label>Email address</Form.Label>
                    <Form.Control type="email" placeholder="Enter email" onChange={(e) => setEmail(e.target.value)} />
                </Form.Group>
                <Form.Group className="mb-3" controlId="formBasicUserName">
                    <Form.Label>Email address</Form.Label>
                    <Form.Control type="text" placeholder="Enter user name" onChange={(e) => setUserName(e.target.value)} />
                </Form.Group>
                <Form.Group className="mb-3" controlId="formBasicPassword">
                    <Form.Label>Password</Form.Label>
                    <Form.Control type="password" placeholder="Enter password" onChange={(e) => setPassword(e.target.value)} />
                </Form.Group>
                <Row>
                    <Col className='d-flex justify-content-center'>
                        <Button variant="primary" type="submit" className='w-50'>
                        Log in
                        </Button>
                    </Col>
                </Row>
                <p className="forgot-password text-right mt-4">
                    Already registered <a href="/login">sign in?</a>
                </p>
            </Form>
            <p ref={errorRef} className={errorMsg
             ? "errmsg"
             : "offscreen"}>
                 {errorMsg}
             </p>
        </section>
    );
}

export default Register;