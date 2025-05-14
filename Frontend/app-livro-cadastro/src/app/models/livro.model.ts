export interface Livro {
    codl: number;
    titulo: string;
    editora: string;
    edicao: number;
    anoPublicacao: string;
    autorIds: number[] | null; 
    assuntoIds: number[] | null; 
    precosFormaCompra: PrecoFormaCompra[] | null;
  }
  
  export interface PrecoFormaCompra {
    formaCompraId: number;
    preco: number;
  }
  