import React, { useState } from 'react';
import Input from '../components/InputComponent';
import Button from '../components/ButtonComponent';
import axios from 'axios';
import '../styles/App.css'

const LoginPage: React.FC = () => {
    const [tag, setTag] = useState('');
    const [password, setPassword] = useState('');

    const handleLogin = async () => {
        try {
            const response = await axios.post('http://localhost:5054/api/auth/login', {
                tag,
                password
            });
            console.log('Login successful', response.data);
            // later here save token and redirect
        } catch (error) {
            console.error('Error logging in: ', error);
        };
    };

    return (
        <div className="p-4">
            <h1 className="text-2xl mb-4">Login</h1>
            <div className="p-4">
                <Input type="text" placeholder="Tag" value={tag} onChange={(e) => setTag(e.target.value)} />
            </div>
             <div className="p-4">
                <Input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
            </div>
            <div className="p-4">
                <Button text="Login" onClick={handleLogin} />
            </div>
        </div>
    );
};


export default LoginPage;