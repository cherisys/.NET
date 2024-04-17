import { useState, useEffect } from 'react';

import { TextField, Button, Box, List, ListItem, ListItemText, ListItemSecondaryAction, IconButton } from '@mui/material';
import { Delete, Edit } from '@mui/icons-material';
function PizzaList({ name, data, onCreate, onUpdate, onDelete, error }) {
    const [formData, setFormData] = useState({ id: '', name: '', description: '' });
    const [editingId, setEditingId] = useState(null);

    useEffect(() => {
        if (editingId === null) {
            setFormData({ id: '', name: '', description: '' });
        } else {
            const currentItem = data.find(item => item.id === editingId);
            setFormData(currentItem);
        }
    }, [editingId, data]);

    const handleFormChange = (event) => {
        const { name, value } = event.target;
        setFormData(prevData => ({
            ...prevData,
            [name]: value,
        }));
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        if (editingId) {
            onUpdate(formData);
        } else {
            onCreate(formData);
        }
        setEditingId(null);
    };

    const handleEdit = (id) => {
        setEditingId(id);
    };

    const handleCancelEdit = () => {
        setEditingId(null);
    };


    return (

        <Box className="Box" sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
            <h2>{name}</h2>
            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'row', alignItems: 'center', gap: 8 }}>
                <TextField label="Name" name="name" value={formData.name} onChange={handleFormChange} />
                <TextField label="Description" name="description" value={formData.description} onChange={handleFormChange} />
                <Button sx={{ mr: 1 }} variant="contained" type="submit">{editingId === null ? 'Create' : 'Update'}</Button>
                {editingId !== null && <Button variant="contained" color="secondary" onClick={handleCancelEdit}>Cancel</Button>}
            </form>
            <List sx={{ width: '100%', maxWidth: 360 }}>
                {data.map(item => (
                    <ListItem key={item.id} secondaryAction={
                        <>
                            <IconButton edge="end" aria-label="edit" onClick={() => handleEdit(item.id)}>
                                <Edit />
                            </IconButton>
                            <IconButton edge="end" aria-label="delete" onClick={() => onDelete(item.id)}>
                                <Delete />
                            </IconButton>
                        </>
                    }>
                        <ListItemText primary={item.name} secondary={item.description} />
                    </ListItem>
                ))}
            </List>
            {error && <p>{error}</p>}
        </Box>

        //<div>
        //    <h2>New {name}</h2>
        //    <form onSubmit={handleSubmit}>
        //        <input
        //            type="text"
        //            name="name"
        //            placeholder="Name"
        //            value={formData.name}
        //            onChange={handleFormChange}
        //        />
        //        <input
        //            type="text"
        //            name="description"
        //            placeholder="Description"
        //            value={formData.description}
        //            onChange={handleFormChange}
        //        />
        //        <button type="submit">{editingId ? 'Update' : 'Create'}</button>
        //        {editingId && <button type="button" onClick={handleCancelEdit}>Cancel</button>}
        //    </form>
        //    {error && <div>{error.message}</div>}
        //    <h2>{name}s</h2>
        //    <ul>
        //        {data.map(item => (
        //            <li key={item.id}>
        //                <div>{item.name} - {item.description}</div>
        //                <div><button onClick={() => handleEdit(item)}>Edit</button>
        //                    <button onClick={() => onDelete(item.id)}>Delete</button></div>
        //            </li>
        //        ))}
        //    </ul>
        //</div>
    );
}

export default PizzaList;