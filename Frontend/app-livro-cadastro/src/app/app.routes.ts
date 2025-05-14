import { Routes } from '@angular/router';
import { LivroListaComponent } from './components/livro/livro-lista/livro-lista.component';
import { AutorListaComponent } from './components/autor/autor-lista/autor-lista.component';
import { AssuntoListaComponent } from './components/assunto/assunto-lista/assunto-lista.component'; // <-- Importar

export const routes: Routes = [
    { path: 'livros', component: LivroListaComponent }, 
    { path: 'autores', component: AutorListaComponent }, 
    { path: 'assuntos', component: AssuntoListaComponent },
];
