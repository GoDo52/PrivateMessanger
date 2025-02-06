import React from 'react';


interface ButtonProps {
    text: string;
    onClick: () => void;
}


const ButtonComponent: React.FC<ButtonProps> = ({ text, onClick }) => {
    return <button onClick={onClick} className="bg-blue-500 text-white p-2 rounded">{text}</button>;
};

export default ButtonComponent;