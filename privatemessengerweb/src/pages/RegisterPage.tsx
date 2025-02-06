import React, { useState } from 'react';
import Input from '../components/InputComponent';
import Button from '../components/ButtonComponent';
import axios from 'axios';
import '../styles/App.css'

const RegisterPage: React.FC = () => {
    const [tag, setTag] = useState('');
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');

    const handleRegister = async () => {
        try {
            const response = await axios.post('http://localhost:5054/api/auth/register', {
                tag,
                userName,
                password
            });
            console.log('Registration successful', response.data);
            // later here save token and redirect
        } catch (error) {
            console.error('Error registering: ', error);
        };
    };

    return (
        <div className="p-4">
            <h1 className="text-2xl mb-4">Register</h1>
            <div className="p-4">
                <Input type="text" placeholder="Tag" value={tag} onChange={(e) => setTag(e.target.value)} />
            </div>
            <div className="p-4">
                <Input type="text" placeholder="Username" value={userName} onChange={(e) => setUserName(e.target.value)} />
            </div>
             <div className="p-4">
                <Input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
            </div>
            <div className="p-4">
                <Button text="Register" onClick={handleRegister} />
            </div>
        </div>
    
    );
};

export default RegisterPage;