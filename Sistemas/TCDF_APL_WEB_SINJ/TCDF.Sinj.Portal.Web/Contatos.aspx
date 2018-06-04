<%@ Page Title="" Language="C#" MasterPageFile="~/Sinj.Master" AutoEventWireup="true" CodeBehind="Contatos.aspx.cs" Inherits="TCDF.Sinj.Portal.Web.Contatos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <div class="divIdentificadorDePagina">
	    <label>.: Contatos </label>
	</div>
    <div class="form">
        <div id="div_contato" class="loaded">
            <fieldset class="w-50-pc">
                <div class="mauto table">
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fl">
                                <label>CLDF: </label>
                            </div>
                        </div>
                        <div class="column w-100-pc" style="padding-bottom: 5px;">
							biblioteca@cl.df.gov.br <br />
                            (61) 3348-9232
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fl">
                                <label>SEPLAG:</label>
                            </div>
                        </div>
                        <div class="column w-100-pc" style="padding-bottom: 5px;">
                            sinj@seplag.df.gov.br <br />
                            (61) 3313-8172
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fl">
                                <label>PGDF:</label>
                            </div>
                        </div>
                        <div class="column w-100-pc" style="padding-bottom: 5px;">
							biblioteca@pg.df.gov.br <br />
                            (61) 3025-9679
                        </div>
                    </div>
                    <div class="line">
                        <div class="column w-30-pc">
                            <div class="cell fl">
                                <label>TCDF:</label>
                            </div>
                        </div>
                        <div class="column w-100-pc" style="padding-bottom: 5px;">
							sinj@tc.df.gov.br  <br />
                            (61) 3314-2226
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
