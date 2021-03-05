/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

USE db_bufunfa;

--
-- Table structure for table `agendamento`
--

DROP TABLE IF EXISTS `agendamento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `agendamento` (
  `IdAgendamento` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID do agendamento',
  `IdUsuario` int(11) NOT NULL COMMENT 'ID do usuário',
  `IdCategoria` int(11) NOT NULL COMMENT 'ID da categoria',
  `IdConta` int(11) DEFAULT NULL COMMENT 'ID da conta',
  `IdCartaoCredito` int(11) DEFAULT NULL COMMENT 'ID do cartão de crédito',
  `IdPessoa` int(11) DEFAULT NULL COMMENT 'ID da pessoa',
  `TipoMetodoPagamento` int(11) NOT NULL COMMENT 'Tipo do método de pagamento:\n1 - Cheque\n2 - Débito\n3 - Depósito\n4 - Transferência\n5 - Dinheiro',
  `Observacao` varchar(500) DEFAULT NULL COMMENT 'Observação sobre o agendamento',
  PRIMARY KEY (`IdAgendamento`),
  KEY `fk_agendamento_usuario_idx` (`IdUsuario`),
  KEY `fk_agendamento_conta_idx` (`IdConta`),
  KEY `fk_agendamento_pessoa_idx` (`IdPessoa`),
  KEY `fk_agendamento_categoria_idx` (`IdCategoria`),
  KEY `fk_agendamento_cartaocredito_idx` (`IdCartaoCredito`),
  CONSTRAINT `fk_agendamento_cartaocredito` FOREIGN KEY (`IdCartaoCredito`) REFERENCES `cartao_credito` (`IdCartaoCredito`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `fk_agendamento_categoria` FOREIGN KEY (`IdCategoria`) REFERENCES `categoria` (`IdCategoria`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_agendamento_conta` FOREIGN KEY (`IdConta`) REFERENCES `conta` (`IdConta`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `fk_agendamento_pessoa` FOREIGN KEY (`IdPessoa`) REFERENCES `pessoa` (`IdPessoa`) ON DELETE SET NULL ON UPDATE NO ACTION,
  CONSTRAINT `fk_agendamento_usuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `atalho`
--

DROP TABLE IF EXISTS `atalho`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `atalho` (
  `IdAtalho` int(11) NOT NULL AUTO_INCREMENT,
  `IdUsuario` int(11) NOT NULL,
  `Titulo` varchar(50) NOT NULL COMMENT 'Título do atalho',
  `Url` varchar(3000) NOT NULL COMMENT 'URL do atalho.',
  PRIMARY KEY (`IdAtalho`),
  KEY `fk_atalho_usuario_idx` (`IdUsuario`),
  CONSTRAINT `fk_atalho_usuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cartao_credito`
--

DROP TABLE IF EXISTS `cartao_credito`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `cartao_credito` (
  `IdCartaoCredito` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID do cartão de crédito',
  `IdUsuario` int(11) NOT NULL COMMENT 'ID do usuário',
  `Nome` varchar(100) NOT NULL COMMENT 'Nome do cartão de crédito',
  `ValorLimite` decimal(10,2) NOT NULL COMMENT 'Valor do limite do cartão de crédito',
  `DiaVencimentoFatura` int(11) NOT NULL COMMENT 'Dia do vencimento da fatura do cartão',
  PRIMARY KEY (`IdCartaoCredito`),
  KEY `fk_cartaocredito_usuario` (`IdUsuario`),
  CONSTRAINT `fk_cartaocredito_usuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `categoria`
--

DROP TABLE IF EXISTS `categoria`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `categoria` (
  `IdCategoria` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID da categoria',
  `IdCategoriaPai` int(11) DEFAULT NULL COMMENT 'ID da categoria pai.',
  `IdUsuario` int(11) DEFAULT NULL COMMENT 'ID do usuário (categorias sem ID do usuário são categorias do sistema)',
  `Tipo` varchar(1) NOT NULL COMMENT 'Tipo da categoria\nC - Crédito\nD - Débito',
  `Nome` varchar(100) NOT NULL COMMENT 'Nome da categoria',
  PRIMARY KEY (`IdCategoria`),
  KEY `fk_categoria_usuario_idx` (`IdUsuario`),
  KEY `fk_categoria_categoria_pai_idx` (`IdCategoriaPai`),
  CONSTRAINT `fk_categoria_categoria_pai` FOREIGN KEY (`IdCategoriaPai`) REFERENCES `categoria` (`IdCategoria`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_categoria_usuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=59 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `conta`
--

DROP TABLE IF EXISTS `conta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `conta` (
  `IdConta` int(11) NOT NULL AUTO_INCREMENT,
  `IdUsuario` int(11) NOT NULL,
  `Nome` varchar(100) NOT NULL COMMENT 'Nome de indentificação da conta (por exemplo "C/C Santander", "BIDI4")',
  `Tipo` int(11) NOT NULL COMMENT 'Tipo da conta:\n1 - Conta corrente\n2 - Poupança\n3 - Renda fixa\n4 - Renda variável',
  `ValorSaldoInicial` decimal(10,2) DEFAULT NULL COMMENT 'Valor do saldo inicial da conta',
  `NomeInstituicao` varchar(500) DEFAULT NULL COMMENT 'Nome da instituição (por exemplo "Banco Santander S/A", "Banco Inter")',
  `NumeroAgencia` varchar(20) DEFAULT NULL COMMENT 'Número da agência',
  `Numero` varchar(20) DEFAULT NULL COMMENT 'Número de identificação da conta.',
  `Ranking` int(11) DEFAULT NULL,
  PRIMARY KEY (`IdConta`),
  KEY `fk_conta_usuario_idx` (`IdUsuario`),
  CONSTRAINT `fk_conta_usuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fatura`
--

DROP TABLE IF EXISTS `fatura`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `fatura` (
  `IdFatura` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID da fatura',
  `IdCartaoCredito` int(11) NOT NULL COMMENT 'ID do cartão de crédito',
  `IdLancamento` int(11) NOT NULL COMMENT 'ID do lançamento',
  `MesAno` varchar(6) NOT NULL COMMENT 'Mês e ano da fatura',
  `ValorAdicionalCredito` decimal(10,2) DEFAULT NULL COMMENT 'Valor adicional de crédito',
  `ObservacaoCredito` varchar(100) DEFAULT NULL COMMENT 'Observação sobre o valor adicional de crédito',
  `ValorAdicionalDebito` decimal(10,2) DEFAULT NULL COMMENT 'Valor adicional de débito',
  `ObservacaoDebito` varchar(100) DEFAULT NULL COMMENT 'Observação sobre o valor adicional de débito',
  PRIMARY KEY (`IdFatura`),
  KEY `fk_fatura_cartaocredito_idx` (`IdCartaoCredito`),
  KEY `fk_fatura_lancamento_idx` (`IdLancamento`),
  CONSTRAINT `fk_fatura_cartaocredito` FOREIGN KEY (`IdCartaoCredito`) REFERENCES `cartao_credito` (`IdCartaoCredito`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `fk_fatura_lancamento` FOREIGN KEY (`IdLancamento`) REFERENCES `lancamento` (`IdLancamento`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `lancamento`
--

DROP TABLE IF EXISTS `lancamento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `lancamento` (
  `IdLancamento` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID do lançamento',
  `IdUsuario` int(11) NOT NULL COMMENT 'ID do usuário',
  `IdConta` int(11) NOT NULL COMMENT 'ID da conta',
  `IdCategoria` int(11) NOT NULL COMMENT 'ID da categoria',
  `IdPessoa` int(11) DEFAULT NULL COMMENT 'ID da pessoa',
  `IdParcela` int(11) DEFAULT NULL COMMENT 'ID da parcela',
  `Data` datetime NOT NULL COMMENT 'Data do lançamento na conta',
  `Valor` decimal(10,2) NOT NULL COMMENT 'Valor do lançamento',
  `QtdRendaVariavel` int(11) DEFAULT NULL COMMENT 'Quantidade de ações (quando a conta for do tipo "Renda Variável")',
  `IdTransferencia` varchar(45) DEFAULT NULL,
  `Observacao` varchar(500) DEFAULT NULL COMMENT 'Observações do lançamento',
  PRIMARY KEY (`IdLancamento`),
  KEY `fk_lancamento_conta_idx` (`IdConta`),
  KEY `fk_lancamento_usuario_idx` (`IdUsuario`),
  KEY `fk_lancamento_categoria_idx` (`IdCategoria`),
  KEY `fk_lancamento_pessoa_idx` (`IdPessoa`),
  KEY `fk_lancamento_parcela_idx` (`IdParcela`),
  CONSTRAINT `fk_lancamento_categoria` FOREIGN KEY (`IdCategoria`) REFERENCES `categoria` (`IdCategoria`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_lancamento_conta` FOREIGN KEY (`IdConta`) REFERENCES `conta` (`IdConta`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `fk_lancamento_parcela` FOREIGN KEY (`IdParcela`) REFERENCES `parcela` (`IdParcela`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `fk_lancamento_pessoa` FOREIGN KEY (`IdPessoa`) REFERENCES `pessoa` (`IdPessoa`) ON DELETE SET NULL ON UPDATE NO ACTION,
  CONSTRAINT `fk_lancamento_usuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=114 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `lancamento_anexo`
--

DROP TABLE IF EXISTS `lancamento_anexo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `lancamento_anexo` (
  `IdAnexo` int(11) NOT NULL AUTO_INCREMENT,
  `IdLancamento` int(11) NOT NULL COMMENT 'ID do lançamento',
  `IdGoogleDrive` varchar(45) NOT NULL COMMENT 'ID do anexo no Google Drive',
  `Descricao` varchar(200) NOT NULL COMMENT 'Descrição do anexo',
  `NomeArquivo` varchar(50) NOT NULL COMMENT 'Nome do arquivo do anexo',
  PRIMARY KEY (`IdAnexo`),
  KEY `fk_anexo_lancamento_idx` (`IdLancamento`),
  CONSTRAINT `fk_anexo_lancamento` FOREIGN KEY (`IdLancamento`) REFERENCES `lancamento` (`IdLancamento`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `lancamento_detalhe`
--

DROP TABLE IF EXISTS `lancamento_detalhe`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `lancamento_detalhe` (
  `IdDetalhe` int(11) NOT NULL AUTO_INCREMENT,
  `IdLancamento` int(11) NOT NULL COMMENT 'ID do lançamento',
  `IdCategoria` int(11) NOT NULL COMMENT 'ID da categoria',
  `Valor` decimal(10,2) NOT NULL COMMENT 'Valor do detalhe do lançamento',
  `Observacao` varchar(500) DEFAULT NULL COMMENT 'Observação do detalhe do lançamento.',
  PRIMARY KEY (`IdDetalhe`),
  KEY `fk_detalhamento_lancamento_idx` (`IdLancamento`),
  KEY `fk_detalhamento_categoria_idx` (`IdCategoria`),
  CONSTRAINT `fk_detalhamento_categoria` FOREIGN KEY (`IdCategoria`) REFERENCES `categoria` (`IdCategoria`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_detalhamento_lancamento` FOREIGN KEY (`IdLancamento`) REFERENCES `lancamento` (`IdLancamento`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `parcela`
--

DROP TABLE IF EXISTS `parcela`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `parcela` (
  `IdParcela` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID da parcela',
  `IdAgendamento` int(11) DEFAULT NULL COMMENT 'ID do agendamento',
  `IdFatura` int(11) DEFAULT NULL COMMENT 'ID da fatura',
  `Data` datetime NOT NULL COMMENT 'Data da parcela',
  `Valor` decimal(10,2) NOT NULL COMMENT 'Valor da parcela',
  `Numero` int(11) NOT NULL,
  `Lancada` bit(1) NOT NULL DEFAULT b'0' COMMENT 'Indica que a parcela foi lançada',
  `DataLancamento` datetime DEFAULT NULL,
  `Descartada` bit(1) NOT NULL DEFAULT b'0' COMMENT 'Indica que a parcela foi descartada',
  `MotivoDescarte` varchar(500) DEFAULT NULL COMMENT 'Descrição do motivo do descarte da parcela',
  `Observacao` varchar(500) DEFAULT NULL COMMENT 'Observação da parcela',
  PRIMARY KEY (`IdParcela`),
  KEY `fk_parcela_fatura_idx` (`IdFatura`),
  KEY `fk_parcela_agendamento_idx` (`IdAgendamento`),
  CONSTRAINT `fk_parcela_agendamento` FOREIGN KEY (`IdAgendamento`) REFERENCES `agendamento` (`IdAgendamento`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `fk_parcela_fatura` FOREIGN KEY (`IdFatura`) REFERENCES `fatura` (`IdFatura`) ON DELETE SET NULL ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=403 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `periodo`
--

DROP TABLE IF EXISTS `periodo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `periodo` (
  `IdPeriodo` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID do período',
  `IdUsuario` int(11) NOT NULL COMMENT 'ID do usuário',
  `Nome` varchar(50) NOT NULL COMMENT 'Nome do período',
  `DataInicio` datetime NOT NULL COMMENT 'Data início do período',
  `DataFim` datetime NOT NULL COMMENT 'Data fim do período',
  PRIMARY KEY (`IdPeriodo`),
  KEY `fk_periodo_usuario_idx` (`IdUsuario`),
  CONSTRAINT `fk_periodo_usuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pessoa`
--

DROP TABLE IF EXISTS `pessoa`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `pessoa` (
  `IdPessoa` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID da pessoa',
  `IdUsuario` int(11) NOT NULL COMMENT 'ID do usuário',
  `Nome` varchar(200) NOT NULL COMMENT 'Nome da pessoa',
  PRIMARY KEY (`IdPessoa`),
  KEY `fk_pessoa_usuario_idx` (`IdUsuario`),
  CONSTRAINT `fk_pessoa_usuario` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `usuario` (
  `IdUsuario` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID do usuário',
  `Nome` varchar(500) NOT NULL COMMENT 'Nome do usuário',
  `Email` varchar(200) NOT NULL COMMENT 'E-mail do usuário',
  `Senha` varchar(500) NOT NULL COMMENT 'Senha do usuário criptografada',
  `Ativo` bit(1) NOT NULL DEFAULT b'1' COMMENT 'Indica se o usuário está ativo',
  PRIMARY KEY (`IdUsuario`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-01-26  0:49:54

-- Senha: admin
INSERT INTO `usuario` (`IdUsuario`, `Nome`, `Email`, `Senha`, `Ativo`) VALUES (1, 'ADMINISTRADOR', 'administrador@bufunfa.net', '21232F297A57A5A743894A0E4A801FC3', 1);
