// Classe para padronização na forma de apresentar feebacks para o usuário
class Feedback {
    constructor(tipo = "INFO", mensagem = "Sua mensagem aqui.", mensagemAdicional = null, tipoAcao = 5) {
        this.tipo = tipo;
        this.mensagem = mensagem;
        this.mensagemAdicional = mensagemAdicional;
        this.tipoAcao = tipoAcao;
    }

    static converter(feedbackJson) {
        if (feedbackJson == null || !feedbackJson.hasOwnProperty("tipoDescricao") || !feedbackJson.hasOwnProperty("mensagem"))
            return null;

        return new Feedback(feedbackJson.tipoDescricao, feedbackJson.mensagem, feedbackJson.hasOwnProperty("mensagemAdicional") ? feedbackJson.mensagemAdicional : null, feedbackJson.hasOwnProperty("tipoAcao") ? feedbackJson.tipoAcao : null);
    }

    exibir(fecharCallback) {
        let swalType = "info";

        switch (this.tipo.toUpperCase()) {
            case "ATENCAO": swalType = "warning"; break;
            case "ERRO": swalType = "error"; break;
            case "SUCESSO": swalType = "success"; break;
        }

        let tipoAcao = this.tipoAcao;

        Swal.fire({
            title: this.mensagem,
            html: this.mensagemAdicional,
            type: swalType,
            allowOutsideClick: false
        }).then(() => {
            if (fecharCallback != null) {
                fecharCallback();
                swal.close();
            }
            else {
                switch (tipoAcao) {
                    case 1: window.history.back(); break;
                    case 2: window.close(); break;
                    case 3: location.href = App.corrigirPathRota("/dashboard"); break;
                    case 4: location.reload(); break;
                    case 5: AppModal.ocultar(); break;
                    case 6: location.href = App.corrigirPathRota("/login"); break;
                }
            }
        });
    }
}