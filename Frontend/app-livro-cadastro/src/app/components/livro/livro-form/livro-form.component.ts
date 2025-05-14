import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Autor } from '../../../models/autor.model';
import { Assunto } from '../../../models/assunto.model';
import { AutorService } from '../../../services/autor.service'; 
import { AssuntoService } from '../../../services/assunto.service';
import { LivroService } from '../../../services/livro.service'; 
import { Router, ActivatedRoute } from '@angular/router';
import { Livro } from '../../../models/livro.model';
import { PrecoFormaCompra } from '../../../models/precoFormaCompra.model';
import { FormaCompra } from '../../../models/formaCompra.model';
import { FormaCompraService } from '../../../services/formaCompra.service';

@Component({
  selector: 'app-livro-form',
  templateUrl: './livro-form.component.html',
  styleUrls: ['./livro-form.component.css']
})
export class LivroFormComponent implements OnInit {
  livroForm: FormGroup;
  autores: Autor[] = [];
  assuntos: Assunto[] = [];
  formasCompra: FormaCompra[] = [];
  livro: Livro = {
    codl: 0,
    titulo: '',
    editora: '',
    anoPublicacao: '',
    autores: [],
    assuntos: [],
    precosFormaCompra: []
  };
  isEditing = false;
  livroId: number = 0;

  constructor(
    private formBuilder: FormBuilder,
    private autorService: AutorService,
    private assuntoService: AssuntoService,
    private livroService: LivroService,
    private router: Router,
    private route: ActivatedRoute,
    private formaCompraService: FormaCompraService
  ) {
    this.livroForm = this.formBuilder.group({
      titulo: ['', Validators.required],
      editora: ['', Validators.required],
      anoPublicacao: ['', Validators.required],
      autorIds: [[], Validators.required],
      assuntoIds: [[], Validators.required],
      precosFormaCompra: this.formBuilder.array([]) // Inicializa como um FormArray
    });
  }

  ngOnInit(): void {
    this.carregarAutores();
    this.carregarAssuntos();
    this.carregarFormasCompra();
    this.route.params.subscribe(params => {
      this.livroId = +params['id'];
      if (this.livroId) {
        this.isEditing = true;
        this.carregarLivro(this.livroId);
      } else {
        this.adicionarPrecoFormaCompra(); // Garante que há pelo menos um grupo de preço inicialmente
      }
    });
  }

  carregarLivro(id: number): void {
    this.livroService.getLivroById(id).subscribe(
      (livro) => {
        this.livro = livro;
        this.livroForm.patchValue({
          titulo: livro.titulo,
          editora: livro.editora,
          anoPublicacao: livro.anoPublicacao,
          autorIds: livro.autores.map(a => a.codAu), // Mapeia de Autor[] para number[]
          assuntoIds: livro.assuntos.map(a => a.codAs) // Mapeia de Assunto[] para number[]
        });

        // Limpa o FormArray de preços antes de adicionar os preços do livro
        const precosFormArray = this.livroForm.get('precosFormaCompra') as FormArray;
        precosFormArray.clear();
        if (livro.precosFormaCompra && livro.precosFormaCompra.length > 0) {
          livro.precosFormaCompra.forEach(preco => {
            precosFormArray.push(this.formBuilder.group({
              formaCompraId: [preco.formaCompraId, Validators.required],
              preco: [preco.preco, Validators.required]
            }));
          });
        }
        else{
          this.adicionarPrecoFormaCompra();
        }
      },
      (error) => {
        console.error('Erro ao carregar livro:', error);
        // Tratar o erro (ex: exibir mensagem ao usuário)
      }
    );
  }

  carregarAutores(): void {
    this.autorService.getAll().subscribe(
      (autores) => {
        this.autores = autores;
      },
      (error) => {
        console.error('Erro ao carregar autores:', error);
        // Tratar o erro
      }
    );
  }

  carregarAssuntos(): void {
    this.assuntoService.getAll().subscribe(
      (assuntos) => {
        this.assuntos = assuntos;
      },
      (error) => {
        console.error('Erro ao carregar assuntos:', error);
        // Tratar o erro
      }
    );
  }

  carregarFormasCompra(): void {
    this.formaCompraService.getAll().subscribe(
      (formasCompra) => {
        this.formasCompra = formasCompra;
      },
      (error) => {
        console.error('Erro ao carregar formas de compra', error);
      }
    );
  }

  getPrecosFormaCompraFormArray(): FormArray {
    return this.livroForm.get('precosFormaCompra') as FormArray;
  }


  adicionarPrecoFormaCompra(): void {
    this.getPrecosFormaCompraFormArray().push(this.formBuilder.group({
      formaCompraId: ['', Validators.required],
      preco: ['', Validators.required]
    }));
  }

  removerPrecoFormaCompra(index: number): void {
    this.getPrecosFormaCompraFormArray().removeAt(index);
  }

  onSubmit(): void {
    if (this.livroForm.valid) {
      const livroData = this.livroForm.value;
      const livroParaSalvar: Livro = {
        codl: this.livroId, // Mantém o ID original para edição
        titulo: livroData.titulo,
        editora: livroData.editora,
        anoPublicacao: livroData.anoPublicacao,
        autores: livroData.autorIds.map((id: number) => this.autores.find(a => a.codAu === id)!), // Converte IDs para objetos Autor
        assuntos: livroData.assuntoIds.map((id: number) => this.assuntos.find(a => a.codAs === id)!), // Converte IDs para objetos Assunto
        precosFormaCompra: livroData.precosFormaCompra
      };

      if (this.isEditing) {
        this.livroService.updateLivro(this.livroId, livroParaSalvar).subscribe(
          () => {
            this.router.navigate(['/livros']);
          },
          (error) => {
            console.error('Erro ao atualizar livro:', error);
            // Tratar erro
          }
        );
      } else {
        this.livroService.addLivro(livroParaSalvar).subscribe(
          () => {
            this.router.navigate(['/livros']);
          },
          (error) => {
            console.error('Erro ao adicionar livro:', error);
            // Tratar erro
          }
        );
      }
    } else {
      console.log("Formulário inválido:", this.livroForm.errors)
      // Exibir mensagem de erro para o usuário
    }
  }

  onCancel(): void {
    this.router.navigate(['/livros']);
  }
}

