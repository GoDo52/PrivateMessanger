import React from 'react';


interface InputProps {
    type: string;
    placeholder: string;
    value: string;
    onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

const InputComponent: React.FC<InputProps> = ({ type, placeholder, value, onChange }) => {
    return <input type={type} placeholder={placeholder} value={value} onChange={onChange} className="border rounded p-2 w-full" />;
};

export default InputComponent;